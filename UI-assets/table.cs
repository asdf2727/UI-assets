using System.Text;

namespace UI_assets;

/*public class UI_Table {
	private struct box_drawing {
		public readonly char vert = '│';
		public readonly char vertB = '║';
		public char hor = '─';
		public char horB = '═';

		public char T = '╤';
		public char start = '╟';
		public char startB = '╠';
		public char end = '╣';
		public char endB = '╢';
		public char plus = '╪';

		public char cornerUL = '╔';
		public char cornerUR = '╗';
		public char cornerBL = '╚';
		public char cornerBR = '╝';

		public box_drawing() { }
	}

	private static readonly box_drawing borders = new();

	private class UI_Table_Line {
		public readonly List<UI_Cell> cells;
		public bool is_done;
		public string line = "";

		public UI_Table_Line(List<UI_Cell> set_cells) {
			cells = set_cells;
		}

		public void Reset() {
			is_done = false;
			foreach (UI_Cell cell in cells) {
				cell.Reset();
			}
		}

		public void Resize(List<int> new_widths) {
			for (int ind = 0; ind < cells.Count; ind++) {
				cells[ind].Resize(new_widths[ind]);
			}
		}

		public string NextLine() {
			StringBuilder line_builder = new();
			line_builder.Append($"{borders.vertB} ");
			is_done = true;
			foreach (UI_Cell cell in cells) {
				line_builder.Append(cell.NextLine());
				line_builder.Append($" {borders.vert} ");
				is_done = is_done && cell.IsDone();
			}
			line_builder.Remove(line_builder.Length - 2, 2);
			line_builder.Append($"{borders.vertB}\n");
			return line = line_builder.ToString();
		}
	}

	private readonly List<UI_Cell> line_template = new();

	public void Reset(List<UI_Cell.align> set_alignments, int col_count, bool has_header) {
		line_template.Clear();
		foreach (UI_Cell.align set_alignment in set_alignments) {
			line_template.Add(new UI_Cell(0, "", set_alignment,));
		}
	}

	public UI_Table(UI_Cell.align[] set_alignments, int col_count, bool has_header) { }
}*/