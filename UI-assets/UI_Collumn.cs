namespace UI_assets; 

public class UI_Collumn {

	private align alignment;
	public align new_alignment;
	private int width;
	public int new_width;
	public int max_len;
	private int ind;

	UI_Collumn(align set_alignment, int set_width, int set_len, int set_ind) {
		alignment = set_alignment;
		width = set_width;
		max_len = set_len;
		ind = set_ind;
	}

	public void ApplyChanges(ref List<UI_Line> lines) {
		foreach (var line in lines) {
			if (width != new_width) {
				line.Resize(ind, new_width);
			}
			if (alignment != new_alignment) {
				line.Realign(ind, new_width);
			}
		}
	}
}