using System.Text;

namespace UI_assets;

/// <summary>
/// A function capable to fit text with markup in multiple lines with a constant defined width. Supports multiple
/// alignments.
/// </summary>
public class UI_Cell {
	private int width;
	public enum align : byte { left, center, right }
	public align alignment;
	
	private int pos = -1;
	private string text; // includes markup
	public string line = "";
	
	private string markup = "/";
	private bool new_markup = false;
	
	private void ReadMarkup() {
		if (pos + 1 < text.Length && text[pos] == '[' && text[pos + 1] != '[') {
			new_markup = true;
			string interpret = "";
			while (pos < text.Length && text[pos] != ']') {
				interpret += text[pos];
				pos++;
			}
			if (!((markup == "/") ^ (interpret == "/"))) {
				throw new Exception();
			}
			markup = interpret;
			pos++;
		}
		// Test for ']]' and '[['
		if (pos < text.Length && text[pos] == ']') {
			pos++;
		}
		if (pos < text.Length && text[pos] == '[') {
			pos++;
		}
	}
	private void NextChar() {
		if (pos < text.Length) {
			pos++;
		}
		ReadMarkup();
	}
	private void SkipSpaces() {
		while (pos < text.Length && text[pos] == ' ') {
			pos++;
		}
	}
	
	public UI_Cell(int set_width, string set_text, align set_alignment) {
		width = set_width;
		text = set_text;
		alignment = set_alignment;
		Reset();
	}

	public void Reset() {
		pos = 0;
		markup = "/";
		new_markup = false;
		ReadMarkup();
		line = "";
	}

	public void Resize(int new_width) {
		width = new_width;
		Reset();
	}

	public string NextLine() {
		int len = 0;
		StringBuilder build_text = new();
		if (markup != "/") {
			// Prepare to add markup if last line had it
			new_markup = true;
		}

		if (pos < text.Length) {
			// State to revert to if a word is broken:
            int last_pos = -1, last_len = -1;
            StringBuilder last_build = new();
            string last_markup = "/";
            
            // Calculate the string part to include
            while (true) {
            	// Save state for all spaces and '\n'
            	if (text[pos] == ' ' || text[pos] == '\n') {
            		last_pos = pos;
            		last_len = len;
            		last_build.Clear(); last_build.Append(build_text);
            		last_markup = ""; last_markup += markup;
            	}
            	// Stop at first '\n'
            	if (text[pos] == '\n') {
            		break;
            	}
            
            	if (new_markup && markup != "/") {
            		build_text.Append('[');
            		build_text.Append(markup);
            		build_text.Append(']');
            		new_markup = false;
            	}
            	len++;
            	build_text.Append(text[pos]);
            	NextChar();
				if (pos >= text.Length || len >= width) {
					break;
				}
            	if (new_markup && markup == "/") {
            		build_text.Append("[/]");
            		new_markup = false;
            	}
            }
            if (last_pos != -1 && pos < text.Length && text[pos] != ' ' && text[pos] != '\n') {
            	// Revert if you have something saved and broke a word
            	pos = last_pos;
            	len = last_len;
            	build_text = last_build;
            	markup = last_markup;
            }
            if (markup != "/" && !new_markup) {
            	// End markup if required
            	build_text.Append("[/]");
            }
            // Skip if next character is '\n'
            SkipSpaces();
            if (pos < text.Length && text[pos] == '\n') {
            	pos++;
            }
		}

		// Add spaces:
		StringBuilder build_line = new();
		switch (alignment) {
		case align.left:
			build_line.Append(build_text);
			build_line.Append(' ', width - len);
			break;
		case align.center:
			build_line.Append(' ', (width - len) / 2);
			build_line.Append(build_text);
			build_line.Append(' ', (width - len + 1) / 2);
			break;
		case align.right:
			build_line.Append(' ', width - len);
			build_line.Append(build_text);
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
		return line = build_line.ToString();
	}

	public bool IsDone() {
		return pos == text.Length;
	}
}