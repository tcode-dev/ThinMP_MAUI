namespace ThinMPm.Views.Text;

public class BaseText : Label
{
    public BaseText()
    {
        LineBreakMode = LineBreakMode.TailTruncation;
        MaxLines = 1;
    }
}
