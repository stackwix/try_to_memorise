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
	//Нужно придумать нормальное название
	public class MyRadioBtn : RadioButton
	{
		public int InfoAboutGameRows { get; set; }
		public int InfoAboutGameColumns { get; set; }
	}

	public partial class MainWindow : Window
	{
		Button[] bts;
		ArrayList userChoises = new ArrayList();
		int[] randomIndexes;

		public MainWindow()
		{
			InitializeComponent();
		}

		//============== Обработчики событий =================
		private void RadioBtn_Checked(object sender, RoutedEventArgs e)
		{
			MyRadioBtn localRadioBtn = (MyRadioBtn)sender;
			CreateNewGameField(localRadioBtn.InfoAboutGameRows, localRadioBtn.InfoAboutGameColumns);
		}
		private void ButtonClick(object sender, RoutedEventArgs e)//Нужно засунуть логику определения победы/поражения в отдельный метод
		{
			
			if (userChoises.Count != randomIndexes.Length) userChoises.Add((Button)sender);
			else
			{
				String str = "Победа!";
				for (int i = 0; i < randomIndexes.Length; i++)
					if (userChoises[i] != bts[randomIndexes[i]])
					{
						str = "Поражение!";
						break;
					}
				MessageBox.Show(str);
				userChoises.Clear();
			}
		}


		//-------------- Методы создания игрового поля с кнопками ------
		public void CreateNewGameField(int rows = 3, int columns = 3)//Здесь временно вызванный метод ген. случ. чисел
		{
			gameGrid.Children.Clear();
			gameGrid.Rows = rows;
			gameGrid.Columns = columns;

			bts = CreateGameButtons(rows * columns);
			AddButtonsToGameField();

			
			GetRandomIndexes(3, gameGrid.Rows * gameGrid.Columns);
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
		public void AddButtonsToGameField()
		{
			for (int i = 0; i < gameGrid.Rows * gameGrid.Columns; i++) gameGrid.Children.Add(bts[i]);
		}
		





		//+++++++++++ Нужно доработать систему появления рандомных ячеек для игрока
		public void GetRandomIndexes(int countOfRandomIndexes, int maxNumber)
		{
			Random rand = new Random();
			randomIndexes = new int[countOfRandomIndexes];

			for (int i = 0; i < countOfRandomIndexes; i++)
			{
				randomIndexes[i] = rand.Next(0, maxNumber);
			}
		}
	}
}
