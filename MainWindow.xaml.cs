using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
	public class MyRadioBtn : RadioButton
	{
		public int InfoAboutGameRows { get; set; }
		public int InfoAboutGameColumns { get; set; }
	}

	public partial class MainWindow : Window
	{
		Button[] bts;
		ArrayList userChoises = new ArrayList();
		//int[] randomIndexes = new int[3];

		public MainWindow()
		{
			InitializeComponent();
		}

		private void RadioBtn_Checked(object sender, RoutedEventArgs e)
		{
			MyRadioBtn localRadioBtn = (MyRadioBtn)sender;
			CreateNewGameField(localRadioBtn.InfoAboutGameRows, localRadioBtn.InfoAboutGameColumns);
		}
		public void CreateNewGameField(int rows = 3, int columns = 3)
		{
			gameGrid.Children.Clear();
			gameGrid.Rows = rows;
			gameGrid.Columns = columns;
			bts = CreateGameButtons(rows * columns);
			AddButtonsToGameField(bts, gameGrid);
		}
		public Button[] CreateGameButtons(int count)
		{
			Button[] bts = new Button[count];
			for (int i = 0; i < count; i++) 
			{
				bts[i] = new Button();
				bts[i].Click += ButtonClick;
			}
			return bts;
		}
		public void AddButtonsToGameField(Button[] bts, UniformGrid ug)
		{
			for (int i = 0; i < ug.Rows * ug.Columns; i++) ug.Children.Add(bts[i]);
		}

		private void ButtonClick(object sender, RoutedEventArgs e)
		{
			if (userChoises.Count != bts.Length) userChoises.Add((Button)sender);
			else
			{
				String str = "Ты победил!";
				for (int i = 0; i < bts.Length; i++)
					if (userChoises[i] != bts[i])
					{
						str = "Ты проиграл!";
						break;
					} 
				MessageBox.Show(str);
				userChoises.Clear();
			}
		}
	}
}
