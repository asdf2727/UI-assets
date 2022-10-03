using System.Text;

namespace UI_assets;

public class UI_Line {
	
	public List<UI_Cell> cells;
	
	public bool is_done;
	public string last_line = "";

	public UI_Line(List<UI_Cell> set_cells) {
		cells = set_cells;
	}

	public void Reset() {
		is_done = true;
		foreach (UI_Cell cell in cells) {
			cell.Reset();
			is_done = is_done && cell.is_done;
		}
	}
	public void Resize(int pos, int new_width) {
		cells[pos].Resize(new_width);
		Reset();
	}
	public void Realign(int pos, align new_alignment) {
		cells[pos].Realign(new_alignment);
		Reset();
	}
	public void Rewrite(int pos, string new_text) {
		cells[pos].Rewrite(new_text);
		Reset();
	}
	public void Resize(List<int> new_widths) {
		is_done = true;
		for (int ind = 0; ind < cells.Count; ind++) {
			cells[ind].Resize(new_widths[ind]);
			is_done = is_done && cells[ind].is_done;
		}
	}
	public void Realign(int pos, int new_width) {
		cells[pos].Resize(new_width);
		is_done = true;
		foreach (UI_Cell cell in cells) {
			is_done = is_done && cell.is_done;
		}
	}
	public void Rewrite(List<string> new_texts) {
		is_done = true;
		for (int ind = 0; ind < cells.Count; ind++) {
			cells[ind].Rewrite(new_texts[ind]);
			is_done = is_done && cells[ind].is_done;
		}
	}

	public string NextLine() {
		StringBuilder line_builder = new();
		line_builder.Append($"{borders.vertB} ");
		is_done = true;
		foreach (UI_Cell cell in cells) {
			line_builder.Append(cell.NextLine());
			line_builder.Append($" {borders.vert} ");
			is_done = is_done && cell.is_done;
		}
		line_builder.Remove(line_builder.Length - 2, 2);
		line_builder.Append(borders.vertB);
		return last_line = line_builder.ToString();
	}
	
}
