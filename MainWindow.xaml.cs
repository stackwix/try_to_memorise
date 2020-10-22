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
		Button[] gameGrid_buttons;
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
		private void Button_Click(object sender, RoutedEventArgs e)//Нужно подумать можно ли улучшить
		{
			if (userChoises.Count != randomIndexes.Length) userChoises.Add((Button)sender);
			if (userChoises.Count == randomIndexes.Length) Enable_GameGrid_ConfirmBttn_CancelBttn(false, true, true);
		}

		private void Start_Click(object sender, RoutedEventArgs e) => StartGame();
		private void Confirm_Click(object sender, RoutedEventArgs e) => ShowResults();
		private void Cancel_Click(object sender, RoutedEventArgs e)
		{
			userChoises.Clear();
			Enable_GameGrid_ConfirmBttn_CancelBttn(true, false, false);
		}


		//-------------- Методы создания игрового поля с кнопками ------
		public void CreateNewGameField(int rows = 3, int columns = 3)
		{
			gameGrid.IsEnabled = false;

			gameGrid.Children.Clear();
			gameGrid.Rows = rows;
			gameGrid.Columns = columns;

			gameGrid_buttons = CreateGameButtons(rows * columns);
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
			for (int i = 0; i < gameGrid.Rows * gameGrid.Columns; i++) gameGrid.Children.Add(gameGrid_buttons[i]);
		}






		// ======== Cистема появления рандомных ячеек для игрока на игров. поле для запоминания и отображение результата ========

		//Метод создает рандомный массив чисел
		public void CreateRandomIndexes(int countOfRandomIndexes, int maxNumber)
		{
			Random rand = new Random();
			randomIndexes = new int[countOfRandomIndexes];

			for (int i = 0; i < countOfRandomIndexes; i++) randomIndexes[i] = rand.Next(0, maxNumber);
		}

		//Метод начала игры
		async public void StartGame()
		{
			LockUI();

			//Очистка памяти
			userChoises.Clear();

			CreateRandomIndexes(5, gameGrid.Rows * gameGrid.Columns);

			//Показывает рандомные для ячейки для запоминания
			for (int i = 0; i < randomIndexes.Length; i++)
			{
				gameGrid_buttons[randomIndexes[i]].Background = new SolidColorBrush(Colors.Red);
				await Task.Delay(1000); //Чтобы игрок успел запомнить засвеченные ячейки
				gameGrid_buttons[randomIndexes[i]].Background = new SolidColorBrush(Colors.White);
				await Task.Delay(1000);
			}

			UnlockUI();
		}
		//Метод отображения успешности в запоминании игроком показаных программой ячеек
		async public void ShowResults()
		{
			
			LockUI();

			for (int i = 0; i < randomIndexes.Length; i++)
			{
				if (userChoises[i] != gameGrid_buttons[randomIndexes[i]])
				{
					gameGrid_buttons[randomIndexes[i]].Background = new SolidColorBrush(Colors.DarkGreen);
					((Button)userChoises[i]).Background = new SolidColorBrush(Colors.Red);
					await Task.Delay(1000);
					gameGrid_buttons[randomIndexes[i]].Background = new SolidColorBrush(Colors.White);
				}
				else
				{
					((Button)userChoises[i]).Background = new SolidColorBrush(Colors.Green);
					await Task.Delay(1000);

				}
				((Button)userChoises[i]).Background = new SolidColorBrush(Colors.White);
				await Task.Delay(1000);
			}

			UnlockUI();
			Enable_GameGrid_ConfirmBttn_CancelBttn(false, false, false);
		}


		//========== Методы блокировки/разблокировки необходимых UI-елементов окна

		//Блокирует необходимые UI-елементы, чтобы пользователь не нажимал на кнопки,
		//на которые НЕ нужно нажимать в данный момент
		public void LockUI()
		{
			EnableGeneralGridChildrens(false);
			foreach (Button bt in gameGrid_buttons) bt.Click -= Button_Click;
			Enable_GameGrid_ConfirmBttn_CancelBttn(true, false, false);
		}
		//Вновь делает доступным необходимый UI для пользователя
		public void UnlockUI()
		{
			foreach (Button bt in gameGrid_buttons) bt.Click += Button_Click;
			EnableGeneralGridChildrens(true);
		}
		public void EnableGeneralGridChildrens(bool isEnabled)
		{
			foreach (FrameworkElement elem in generalGrid.Children) elem.IsEnabled = isEnabled;
		}
		public void Enable_GameGrid_ConfirmBttn_CancelBttn(bool gameGridIsEnabled, bool confirmIsEnabled, bool cancelIsEnabled)
		{
			gameGrid.IsEnabled = gameGridIsEnabled;
			confirm.IsEnabled = confirmIsEnabled;
			cancel.IsEnabled = cancelIsEnabled;
		}
	}	
}
