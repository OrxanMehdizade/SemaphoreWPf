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
        private int ThreadNumber = 1;
        private List<string> CreatedThreads = new List<string>(); //<<yaradilan Threadler siyahisi 
        private List<string> WaitingThreads = new List<string>(); //<<gozliyen Threadler siyahisi
        private List<string> WorkingThreads = new List<string>(); //<<isliyen  Threadler siyahisi
        private Semaphore semaphore;
        private int WorkingSemaphoreSize = 0;

        public MainWindow()
        {
            InitializeComponent();
            semaphore = new Semaphore(0, int.MaxValue);
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            string ThreadName = $"Thread {ThreadNumber}";
            CreatedThreads.Add(ThreadName);
            ListBoxItem newItem = new ListBoxItem();
            newItem.Content = ThreadName;
            CreatedThreadsListBox.Items.Add(newItem);
            ThreadNumber++;

            AddThreadsMethodlist(ThreadName);
        }

        private void WorkingThreadsListBox_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ListBox listBox = (ListBox)sender;
            ListBoxItem secdiyimitem = (ListBoxItem)listBox.SelectedItem;
            if (secdiyimitem != null)
            {
                string workingThreadName = secdiyimitem.Content.ToString();
                if (WorkingThreads.Contains(workingThreadName))
                {
                    WorkingThreads.Remove(workingThreadName);
                    listBox.Items.Remove(secdiyimitem);
                    if (WaitingThreads.Count > 0)
                    {
                        string newThreadName = WaitingThreads[0];
                        WaitingThreads.RemoveAt(0);

                        ListBoxItem workingItem = new ListBoxItem();
                        workingItem.Content = newThreadName;
                        WorkingThreadsListBox.Items.Add(workingItem);
                        WorkingThreads.Add(newThreadName);
                        SemaphoreSizeIncrementMethod();
                    }

                }
            }

        }
        private void CreatedThreadsListBox_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ListBox listBox = (ListBox)sender;
            ListBoxItem secdiyimitem = (ListBoxItem)listBox.SelectedItem;
            if (secdiyimitem != null)
            {
                string CreatedThreadName = secdiyimitem.Content.ToString();
                if (CreatedThreads.Contains(CreatedThreadName))
                {
                    CreatedThreads.Remove(CreatedThreadName);
                    ListBoxItem gozliyenItem = new ListBoxItem();
                    gozliyenItem.Content = CreatedThreadName;
                    WaitingThreadsListBox.Items.Add(gozliyenItem);
                    WaitingThreads.Add(CreatedThreadName);
                }

            }
        }
        private void WaitingThreadsListBox_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ListBox listBox = (ListBox)sender;
            ListBoxItem secdiyimitem = (ListBoxItem)listBox.SelectedItem;
            if (secdiyimitem != null)
            {
                string WaitingThreadName = secdiyimitem.Content.ToString();
                if (WaitingThreads.Contains(WaitingThreadName))
                {
                    WaitingThreads.Remove(WaitingThreadName);
                    AddThreadsMethodlist(WaitingThreadName);



                }
            }

        }
        private void AddThreadsMethodlist(string ThreadName)
        {
            if (WorkingSemaphoreSize > 0)
            {
                ListBoxItem WorkingItem=new ListBoxItem();
                WorkingItem.Content = ThreadName;
                WorkingThreadsListBox.Items.Add(WorkingItem);
                WorkingThreads.Add(ThreadName);
                SemaphoreSizeDecrementMethod();
            }
            else
            {
                ListBoxItem WaitingItem= new ListBoxItem();
                WaitingItem.Content = ThreadName;
                WaitingThreadsListBox.Items.Add(WaitingItem);
                WaitingThreads.Add(ThreadName);
            }
            Thread aftoThread = new Thread(() => { AftoThreadsMethodcheck(); });
            aftoThread.Start();

        }

        private void AftoThreadsMethodcheck() 
        {
            while (WaitingThreads.Count > 0 && WorkingSemaphoreSize>0) 
            {
                var aftoThreadName = WaitingThreads[0];
                Dispatcher.Invoke(() =>
                {
                    ListBoxItem secilenItem= WaitingThreadsListBox.ItemContainerGenerator.
                    ContainerFromIndex(0) as ListBoxItem;
                    if(secilenItem != null )
                    {
                        WaitingThreadsListBox.SelectedItem = secilenItem;
                    }
                });
                Thread.Sleep(TimeSpan.FromSeconds(1));
                Dispatcher.Invoke(() => { WorkingThreadsListBox_MouseDoubleClick(WaitingThreadsListBox, null); });
            }


        }

        public void SemaphoreSizeIncrementMethod()
        {
            WorkingSemaphoreSize++;
            SemaphoreCountTextBox.Text=WorkingSemaphoreSize.ToString();

        }
        public void SemaphoreSizeDecrementMethod()
        {
            WorkingSemaphoreSize--;
            SemaphoreCountTextBox.Text = WorkingSemaphoreSize.ToString();
        }

        private void IncreaseButton_Click(object sender, RoutedEventArgs e)
        {
            SemaphoreSizeIncrementMethod();
        }

        private void DecreaseButton_Click(object sender, RoutedEventArgs e)
        {
            SemaphoreSizeDecrementMethod();
        }





    }
}