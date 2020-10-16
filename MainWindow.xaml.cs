using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading;
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
		private void Button_Click(object sender, RoutedEventArgs e)//Нужно засунуть логику определения победы/поражения в отдельный метод
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
		private void Start_Click(object sender, RoutedEventArgs e)
		{
			StartNewGame();
		}

		//-------------- Методы создания игрового поля с кнопками ------
		public void CreateNewGameField(int rows = 3, int columns = 3)
		{
			gameGrid.Children.Clear();
			gameGrid.Rows = rows;
			gameGrid.Columns = columns;

			bts = CreateGameButtons(rows * columns);
			AddButtonsToGameField();
		}
		public Button[] CreateGameButtons(int count)
		{
			Button[] bts = new Button[count];
			for (int i = 0; i < count; i++) 
			{
				bts[i] = new Button();
				bts[i].Background = new SolidColorBrush(Colors.White);
				bts[i].Click += Button_Click;
			}
			return bts;
		}
		public void AddButtonsToGameField()
		{
			for (int i = 0; i < gameGrid.Rows * gameGrid.Columns; i++) gameGrid.Children.Add(bts[i]);
		}
		





		//+++++++++++ Нужно доработать систему появления рандомных ячеек для игрока
		public void CreateRandomIndexes(int countOfRandomIndexes, int maxNumber)
		{
			Random rand = new Random();
			randomIndexes = new int[countOfRandomIndexes];

			for (int i = 0; i < countOfRandomIndexes; i++)
			{
				randomIndexes[i] = rand.Next(0, maxNumber);
			}
		}

		async public void StartNewGame()//Здесь нужен рефакторинг!
		{
			foreach (FrameworkElement elem in generalGrid.Children) elem.IsEnabled = false;
			gameGrid.IsEnabled = true;

			CreateRandomIndexes(5, gameGrid.Rows * gameGrid.Columns);

			for (int i = 0; i < randomIndexes.Length; i++)
			{
				bts[randomIndexes[i]].Background = new SolidColorBrush(Colors.Red);
				await Task.Delay(1000);
				bts[randomIndexes[i]].Background = new SolidColorBrush(Colors.White);
				await Task.Delay(1000);
			}

			foreach (FrameworkElement elem in generalGrid.Children) elem.IsEnabled = true;
			
		}
	}
}
