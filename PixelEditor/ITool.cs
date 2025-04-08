public interface ITool
{
    void MouseDown(MouseEventArgs e);
    void MouseMove(MouseEventArgs e);
    void MouseUp(MouseEventArgs e);
    void DrawPreview(Graphics g);
}