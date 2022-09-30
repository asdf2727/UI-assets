using UI_assets;

UI_Cell test = new(10, "This i[red]s a test strin[/]g with reallyreallylongwords and\nbreaks\n\nand empty lines.", UI_Cell.align.center);
while (!test.IsDone()) {
	Console.WriteLine($"| {test.NextLine()} |");
}
Console.WriteLine($"| {test.NextLine()} |");