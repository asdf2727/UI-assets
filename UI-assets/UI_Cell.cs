using System.ComponentModel.Design.Serialization;
using System.Text;

namespace UI_assets;

/// <summary>
/// A class capable to fit text with markup in multiple lines with a constant defined width. Supports multiple
/// alignments.
/// </summary>
public class UI_Cell {
	
	private class State {
		public int pos, len;
		public StringBuilder built_text = new();
		public String markup, prev_markup;

		public State(State to_copy) {
			pos = to_copy.pos;
			len = to_copy.len;
			built_text.Clear();
			built_text.Append(to_copy.built_text);
			markup = "" + to_copy.markup;
			prev_markup = "" + to_copy.prev_markup;
		}

		public State(int set_pos, string set_markup) {
			pos = set_pos;
			len = 0;
			built_text.Clear();
			markup = set_markup;
			prev_markup = "/";
		}
	}
	
	private int width;
	public align alignment;
	private string text; // includes markup
	
	private int line_start;
	private string start_markup;
	public bool is_done;
	public string last_line; // includes markup

	private void ReadMarkup(ref int pos, ref string markup) {
		while (pos + 1 < text.Length && text[pos] == '[' && text[pos + 1] != '[') {
			StringBuilder interpret = new();
			pos++;
			while (pos < text.Length && text[pos] != ']') {
				interpret.Append(text[pos]);
				pos++;
			}
			markup = interpret.ToString();
			pos++;
		}
		if (pos < text.Length && (text[pos] == '[' || text[pos] == ']')) {
			pos++;
		}
	}
	private void NextChar(ref int pos, ref string markup) {
		if (pos < text.Length) {
			pos++;
			ReadMarkup(ref pos, ref markup);
		}
	}
	private void SkipSpaces(ref int pos, ref string markup) {
		while (pos < text.Length && text[pos] == ' ') {
			NextChar(ref pos, ref markup);
		}
	}
	
	public UI_Cell(int set_width, string set_text, align set_alignment) {
		width = set_width;
		text = set_text;
		alignment = set_alignment;
		Reset();
	}

	public void Reset() {
		line_start = 0;
		start_markup = "/";
		is_done = (text.Length == 0);
		ReadMarkup(ref line_start, ref start_markup);
		last_line = "";
	}
	public void Resize(int new_width) {
		width = new_width;
		Reset();
	}
	public void Realign(align new_alignment) {
		alignment = new_alignment;
		Reset();
	}
	public void Rewrite(string set_text) {
		text = set_text;
		Reset();
	}

	public string NextLine() {
		State now = new State(line_start, start_markup);

		// Calculate the string part to include
		if (!is_done) {
			char prev_char = 'F';
			State last_save = new State(-1, "/"); // State to revert to if a word is broken:
			
            while (now.pos < text.Length && now.len < width) {
            	// Save state for all beggining spaces and '\n'
            	if (text[now.pos] == '\n' || (text[now.pos] == ' ' && prev_char != ' ')) {
					// TODO: optimize maybe with a '=' operator to avoid leaks
					last_save = new State(now);
				}
            	// Stop at first '\n'
            	if (text[now.pos] == '\n') {
            		break;
            	}
				// Add markup
            	if (now.prev_markup != now.markup) {
					if (now.prev_markup != "/" && now.markup != "/") {
						now.built_text.Append("[/]");
					}
					now.built_text.Append($"[{now.markup}]");
            	}
				// Add current char
				now.built_text.Append(text[now.pos]);
				if (text[now.pos] == '[' || text[now.pos] == ']') {
					now.built_text.Append(text[now.pos]);
				}
				now.len++;
				// Get next char
				prev_char = text[now.pos];
				now.prev_markup = now.markup;
				NextChar(ref now.pos, ref now.markup);
			}
			
			// Revert if you have something saved and broke a word
            if (last_save.pos != -1 && now.pos < text.Length && text[now.pos] != ' ' && text[now.pos] != '\n') {
				now = new State(last_save);
			}
			// End markup if required
            if (now.prev_markup != "/") {
				now.built_text.Append("[/]");
            }
            // Skip if next character is '\n'
            SkipSpaces(ref now.pos, ref now.markup);
            if (now.pos < text.Length && text[now.pos] == '\n') {
				now.pos++;
            }
			// Prepare member variables for next line
			line_start = now.pos;
			start_markup = now.markup;
			is_done = (line_start == text.Length);
		}

		// Add spaces:
		StringBuilder build_line = new();
		switch (alignment) {
		case align.left:
			build_line.Append(now.built_text);
			build_line.Append(' ', width - now.len);
			break;
		case align.center:
			build_line.Append(' ', (width - now.len) / 2);
			build_line.Append(now.built_text);
			build_line.Append(' ', (width - now.len + 1) / 2);
			break;
		case align.right:
			build_line.Append(' ', width - now.len);
			build_line.Append(now.built_text);
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
		return last_line = build_line.ToString();
	}
	
}