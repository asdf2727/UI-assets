using UI_assets;
using Spectre.Console;

string str = "[red]This is a test string[/][green] wi[/][yellow]th reallyreallylongwords[/] [red]\nand breaks\n\nand [/][[]]emp[blue]ty lines.[/]";

List<UI_Cell> cell_list = new();
cell_list.Add(new UI_Cell(21, str, align.center));
cell_list.Add(new UI_Cell(10, str, align.left));
cell_list.Add(new UI_Cell(11, str, align.right));
cell_list.Add(new UI_Cell(15, str, align.left));
UI_Line line = new(cell_list);

while (!line.is_done) {
	line.NextLine();
	//AnsiConsole.WriteLine(line.last_line);
	AnsiConsole.MarkupLine(line.last_line);
}