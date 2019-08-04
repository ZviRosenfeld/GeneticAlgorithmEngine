using System.Windows.Forms;

namespace GUI
{
    public partial class DoubleInputBox : UserControl
    {
        public DoubleInputBox()
        {
            InitializeComponent();
        }
        
        public double GetValue => double.Parse(InputBox.Text) / 100;

        public string LableText
        {
            get => Lable.Text;
            set => Lable.Text = value;
        }
    }
}
