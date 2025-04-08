public class ToolSettings
{
    public Color DrawColor { get; set; } = Color.Black;
    public int PenSize { get; set; } = 3;
    public Font TextFont { get; set; } = new Font("Arial", 12);
    public Color TextColor { get; set; } = Color.Black;
    public Color TextBgColor { get; set; } = Color.White;
    public bool UseTextBackground { get; set; } = true;
}