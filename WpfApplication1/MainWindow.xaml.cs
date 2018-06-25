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
using System.Data.SqlClient;
using System.Data;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void Task2_1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection connection =
                new SqlConnection(@"Data Source=" + TextboxDataSource.Text + "; Initial Catalog=testDB; Integrated Security=SSPI;"))
                {
                    connection.Open();

                        new SqlCommand(@"create table gruppa (
                    Id INT not null primary key identity,
                    Name varchar(50) not null )", connection).ExecuteNonQuery();
                        MessageBox.Show("Table created!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void Task3_Click(object sender, RoutedEventArgs e)
        {
            SqlConnectionStringBuilder connectionString = new SqlConnectionStringBuilder();
            connectionString.DataSource = TextboxDataSource.Text;
            connectionString.InitialCatalog = "testDB";
            connectionString.UserID = "Kostya";
            connectionString.Password = "itstep";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString.ConnectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand();
                    command.CommandText = @"create table gruppa (
                    Id INT not null primary key identity,
                    Name varchar(50) not null )";
                    command.Connection = connection;
                        command.ExecuteNonQuery();
                        MessageBox.Show("Table created!");
                
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
}

        private void DropTableButton(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection connection =
                new SqlConnection(@"Data Source=" + TextboxDataSource.Text + "; Initial Catalog=testDB; Integrated Security=SSPI;"))
                {
                    connection.Open();

                    new SqlCommand(@"drop table gruppa", connection).ExecuteNonQuery();
                        MessageBox.Show("Table successfully dropped!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void MagicButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection connection =
                new SqlConnection(@"Data Source=" + TextboxDataSource.Text + "; Initial Catalog=master; Integrated Security=SSPI;"))
                {
                    connection.Open();

                    new SqlCommand(@"create database Sales", connection).ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
			
            List<string> commands = new List<string>();
            commands.Add(
                @"create table Customers ( id INT not null primary key identity,
                firstName varchar(30) not null,
                lastName varchar(30) not null)");
            commands.Add(
                @"create table Sellers ( id INT not null primary key identity,
                firstName varchar(30) not null,
                lastName varchar(30) not null)");
            commands.Add(
                @"create table Sales ( id INT not null primary key identity,
                customer_id INT FOREIGN KEY REFERENCES Customers(id),
                seller_id INT FOREIGN KEY REFERENCES Sellers(id),
                amount MONEY not null,
                date_of_deal date not null)");
            commands.Add(
                @"insert into Customers VALUES ('John', 'Lennon'), ('Kostya', 'Gudmundsson'), ('Michael', 'Galushko'), ('Piotr', 'Zielinski');");
            commands.Add(
                @"insert into Sellers VALUES ('Harvy', 'Weinstein'), ('Valera', 'Adilson'), ('Pablo', 'Escobar'), ('Mohamed', 'Elfathy');");
            commands.Add(
                @"insert into Sales VALUES (1, 1, 350000, '2017-11-03'), (2, 2, 6100, '2017-12-04'), (3, 3, 600000, '1985-10-10'), (4, 4, 10, '2018-02-03'), (1, 3, 15000, '1978-04-01');");


            try
            {
                using (SqlConnection connection =
                new SqlConnection(@"Data Source=" + TextboxDataSource.Text + "; Initial Catalog=Sales; Integrated Security=SSPI;"))
                {
                    connection.Open();

                    foreach (string item in commands)
                    {
                        try
                        {
                            new SqlCommand(item, connection).ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Error");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }
        
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection connection =
                new SqlConnection(@"Data Source=" + TextboxDataSource.Text + "; Initial Catalog=Sales; Integrated Security=SSPI;"))
                {
                    connection.Open();

                    SqlDataReader reader = new SqlCommand(@"SELECT name FROM sys.objects WHERE type in (N'U')", connection).ExecuteReader();
                    
                    while(reader.Read())
                    {
                        tableList.Items.Add(reader.GetValue(0));
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
            SizeToContent = SizeToContent.Height;
        }
        private void tableList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                using (SqlConnection connection =
                new SqlConnection(@"Data Source=" + TextboxDataSource.Text + "; Initial Catalog=Sales; Integrated Security=SSPI;"))
                {
                    connection.Open();

                    DataTable dt = new DataTable("test");
                    new SqlDataAdapter(new SqlCommand(@"select * from " + tableList.SelectedItem.ToString(), connection)).Fill(dt);
                    listView.ItemsSource = dt.DefaultView;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
            SizeToContent = SizeToContent.Height;

        }
    }
}
