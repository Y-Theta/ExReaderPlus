using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using EFCore.DB;
using Microsoft.EntityFrameworkCore;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace EFCore {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page {
        public MainPage() {
            this.InitializeComponent();
        }

        private async void MigrateButton_OnClick(object sender,
            RoutedEventArgs e) {
            using (var db = new DataContext()) {
                await db.Database.MigrateAsync();
            }
        }

        private async void InsertButton_OnClick(object sender,
            RoutedEventArgs e) {
            using (var db = new DataContext()) {
                var student = new Student {Name = "S1"};
                db.Students.Add(student);
                await db.SaveChangesAsync();

                var mission = new Mission {Description = "M1"};
                db.Missions.Add(mission);
                await db.SaveChangesAsync();

                var sm = new StudentMission
                    {Student = student, Mission = mission};
                db.StudentMissions.Add(sm);
                await db.SaveChangesAsync();

                var sms = db.StudentMissions.Include(p => p.Mission)
                    .Include(p => p.Student)
                    .Where(p => p.Mission.Description.Contains("Hello"))
                    .ToList();
                foreach (var studentMission in sms) {
                    // studentMission.Student.Name;
                }

                var sms2 = db.Missions
                    .Where(p => p.Description.Contains("Hello"))
                    .Include(p => p.StudentMissions).ThenInclude(p => p.Student)
                    .First(); // LINQ

                var missions = db.Missions.Include(p => p.StudentMissions)
                    .ThenInclude(p => p.Student).ToList();
                foreach (var mission1 in missions) {
                    if (mission1.Description.Contains("Hello")) {
                        // mission1
                        // ORM Object Relation Mapping
                    }
                }

                // sms2.StudentMissions[0].Student.Name;

                foreach (var sms2StudentMission in sms2.StudentMissions) {
                    //sms2StudentMission.Student.Name;
                }
            }
        }
    }
}