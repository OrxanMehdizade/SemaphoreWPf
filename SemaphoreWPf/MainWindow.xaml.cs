using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace SemaphoreWPf
{
    public partial class MainWindow : Window
    {
        private int serialNumber = 1; // İş parçacığı seri numarası
        private List<string> createdThreads = new List<string>(); // Oluşturulan iş parçacıkları listesi
        private List<string> workingThreads = new List<string>(); // Çalışan iş parçacıkları listesi
        private List<string> waitingThreads = new List<string>(); // Bekleyen iş parçacıkları listesi
        private Semaphore semaphore; // Semaphore nesnesi
        private int semaphoreCount = 0; // Semaphore izin sayısı

        public MainWindow()
        {
            InitializeComponent();
            semaphore = new Semaphore(0, int.MaxValue); // Başlangıçta 0 olarak ayarlanmış bir Semaphore oluşturulur
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            string threadName = "Thread " + serialNumber; // Yeni iş parçacığı adı oluştur
            createdThreads.Add(threadName); // Oluşturulan iş parçacığını listeye ekle

            // Yeni iş parçacığını ilk liste (oluşturulan tüm konular) ListBox'ına ekle
            ListBoxItem newItem = new ListBoxItem();
            newItem.Content = threadName;
            CreatedThreadsListBox.Items.Add(newItem);

            serialNumber++; // İş parçacığı seri numarasını artır

            IncreaseSemaphoreCount(); // Semaphore izin sayısını artır
        }

        private void WorkingThreadsListBox_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ListBox listBox = (ListBox)sender;
            ListBoxItem selectedItem = (ListBoxItem)listBox.SelectedItem;

            if (selectedItem != null)
            {
                string threadName = selectedItem.Content.ToString();

                if (waitingThreads.Contains(threadName))
                {
                    waitingThreads.Remove(threadName); // Bekleyen iş parçacığını listeden kaldır

                    // İş parçacığını çalışanlar listesine ekle
                    ListBoxItem workingItem = new ListBoxItem();
                    workingItem.Content = threadName;
                    WorkingThreadsListBox.Items.Add(workingItem);
                    workingThreads.Add(threadName);

                    DecreaseSemaphoreCount(); // Semaphore izin sayısını azalt

                    CheckSemaphoreAvailability(); // Semaphore durumunu kontrol et
                }
            }
        }


        private void CreatedThreadsListBox_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ListBox listBox = (ListBox)sender;
            ListBoxItem selectedItem = (ListBoxItem)listBox.SelectedItem;

            if (selectedItem != null)
            {
                string threadName = selectedItem.Content.ToString();

                if (createdThreads.Contains(threadName))
                {
                    createdThreads.Remove(threadName); // Oluşturulan iş parçacığını listeden kaldır

                    // İş parçacığını bekleyenler listesine ekle
                    ListBoxItem waitingItem = new ListBoxItem();
                    waitingItem.Content = threadName;
                    WaitingThreadsListBox.Items.Add(waitingItem);
                    waitingThreads.Add(threadName);

                    //CheckSemaphoreAvailability(); // Semaphore durumunu kontrol et
                }
            }
        }

        private void WaitingThreadsListBox_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ListBox listBox = (ListBox)sender;
            ListBoxItem selectedItem = (ListBoxItem)listBox.SelectedItem;

            if (selectedItem != null)
            {
                string threadName = selectedItem.Content.ToString();

                if (waitingThreads.Contains(threadName))
                {
                    waitingThreads.Remove(threadName); // Bekleyen iş parçacığını listeden kaldır

                    // İş parçacığı kendini çalışanlar listesine eklesin
                    ListBoxItem workingItem = new ListBoxItem();
                    workingItem.Content = threadName;
                    WorkingThreadsListBox.Items.Add(workingItem);
                    workingThreads.Add(threadName);

                    // İş parçacığı kendini çalışanlar listesine ekledikten sonra süreyle beklesin
                    Timer timer = new Timer((state) =>
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            // Süre sonunda iş parçacığı kendini çalışanlar listesinden kaldırıp Semaphore izin sayısını artırsın
                            WorkingThreadsListBox.Items.Remove(workingItem);
                            workingThreads.Remove(threadName);

                            IncreaseSemaphoreCount(); // Semaphore izin sayısını artır

                            CheckSemaphoreAvailability(); // Semaphore durumunu kontrol et
                        });
                    }, null, TimeSpan.FromSeconds(1), Timeout.InfiniteTimeSpan);
                }
            }
        }

        private void CheckSemaphoreAvailability()
        {
            if (semaphoreCount > 0 && waitingThreads.Count > 0)
            {
                int threadsToMove = Math.Min(semaphoreCount, waitingThreads.Count);

                foreach (string threadName in waitingThreads.Take(threadsToMove).ToList())
                {
                    // Bekleyen iş parçacığını çalışanlar listesine taşı
                    ListBoxItem workingItem = new ListBoxItem();
                    workingItem.Content = threadName;
                    WorkingThreadsListBox.Items.Add(workingItem);
                    workingThreads.Add(threadName);

                    waitingThreads.Remove(threadName);
                    DecreaseSemaphoreCount(); // Semaphore izin sayısını azalt
                }
            }
        }

        private void IncreaseSemaphoreCount()
        {
            semaphoreCount++;
            SemaphoreCountTextBox.Text = semaphoreCount.ToString();
        }

        private void DecreaseSemaphoreCount()
        {
            if (semaphoreCount > 0)
            {
                semaphoreCount--;
                SemaphoreCountTextBox.Text = semaphoreCount.ToString();
            }
        }

        private void IncreaseButton_Click(object sender, RoutedEventArgs e)
        {
            IncreaseSemaphoreCount();
        }

        private void DecreaseButton_Click(object sender, RoutedEventArgs e)
        {
            DecreaseSemaphoreCount();
        }
    }
}