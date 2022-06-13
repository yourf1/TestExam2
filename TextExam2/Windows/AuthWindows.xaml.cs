using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TextExam2.Model;
using TextExam2.Windows;

namespace TextExam2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //При открытие окна ресурсы обнуляются
            Application.Current.Resources["Role"] = "null";
            Application.Current.Resources["FullName"] = "Гость";
            Application.Current.Resources["UserId"] = "null";
        }

        public int Capch = 0;

        //Показ капчи
        void VievCapch()
        {
            CapchTbx.Visibility = Visibility.Visible;
            CapchImage.Visibility = Visibility.Visible;
        }

        //Скрытие капчи
        void HideCapch()
        {
            CapchTbx.Visibility = Visibility.Hidden;
            CapchImage.Visibility = Visibility.Hidden;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Проверка на заполненность полей ввода данных
            if (String.IsNullOrEmpty(LoginTbx.Text) || String.IsNullOrEmpty(PassPbx.Password)) {
                MessageBox.Show("Вы не ввели все данные!");
                return;
            }
            
            //Проверка на капчу
            if(Capch > 0)
            {
                if (CapchTbx.Text != "1234")
                {
                    MessageBox.Show("Капча не верная");
                    return;
                }
            }

            //Нужно обернурь в тру кейч чтобы не сломалось в случае отключания бд
            try
            {
                //Авторизация 
                var AuthUser = ClassConnect.com.User.Where(a => a.Login == LoginTbx.Text && a.Password == PassPbx.Password.ToString());
                if (AuthUser.Count() > 0)
                {
                    //MessageBox.Show("Успешно");

                    //Заполнение ресурсов
                    var DataAuth = AuthUser.ToList()[0];
                    Application.Current.Resources["Role"] = DataAuth.IdRole;
                    Application.Current.Resources["FullName"] = DataAuth.FirtName + " " + DataAuth.LastName;
                    Application.Current.Resources["UserId"] = DataAuth.Id;

                    //Открытие нового окна
                    ConfigWindow newWendow = new ConfigWindow();
                    newWendow.Show();
                    this.Close();

                    HideCapch();
                }
                else
                {
                    MessageBox.Show("Данные не верные!");
                    Capch++;
                    VievCapch();
                }
            }
            catch
            {
                MessageBox.Show("Отвалилась бд");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //Заполняем ресурс роль = 1, чтобы были права как у книета (клиент = гость)
            Application.Current.Resources["Role"] = "1";
            ConfigWindow newWendow = new ConfigWindow();
            newWendow.Show();
            this.Close();
        }
    }
}
