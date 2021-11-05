using Mini_Task_Manager.Commands;
using Mini_Task_Manager.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Mini_Task_Manager.ViewModel
{
	public class MainViewModel:BaseViewModel
	{
		public MainWindow MainWindow { get; set; }
        public ObservableCollection<Process> Processes { get; set; }
        private Process selectedProcess;

        public Process SelectedProcess
        {
            get { return selectedProcess; }
            set { selectedProcess = value; }
        }

        public RelayCommand AddCommand { get; set; }
        public RelayCommand CreateCommand { get; set; }
        public RelayCommand EndCommand { get; set; }

        public MainViewModel(MainWindow window)
        {
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Interval = TimeSpan.FromSeconds(5);
            dispatcherTimer.Start();

            MainWindow = window;
            AddCommand = new RelayCommand((sender => {
                window.WishListView.Items.Add(window.WishListName.Text);
            }));

            CreateCommand = new RelayCommand((sender => {
                if (window.WishListView.Items == null)
                {
                    Process.Start(window.ProcessNameListName.Text);
                }
                else
                {
                    try
                    {
                    Process.Start(window.ProcessNameListName.Text);
                    Thread.Sleep(5000);
                    foreach (var item in window.WishListView.Items)
                    {
                        var process = Processes.FirstOrDefault(pr => pr.ProcessName == item.ToString());
                        process.Kill();
                    }
                    }
                    catch (Exception err)
                    {
                        Console.WriteLine(err.Message);
                    }
                }
            }));

            EndCommand = new RelayCommand((sender =>
            {
                try
                {
                    if (SelectedProcess != null)
                    {
                        SelectedProcess.Kill();
                    }
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message);
                }

            }));
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            Processes = new ObservableCollection<Process>(Process.GetProcesses());
        }
    }
}
