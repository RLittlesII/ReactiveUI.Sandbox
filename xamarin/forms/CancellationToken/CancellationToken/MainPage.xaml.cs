﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI.XamForms;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CancellationToken
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ReactiveContentPage<MainViewModel>
    {
        public MainPage()
        {
            InitializeComponent();

            var connectivityChanges =
                Observable
                    .FromEvent<EventHandler<ConnectivityChangedEventArgs>, ConnectivityChangedEventArgs>(eventHandler =>
                        {
                            void Handler(object sender, ConnectivityChangedEventArgs eventArgs) =>
                                eventHandler(eventArgs);
                            return Handler;
                        },
                        x => Connectivity.ConnectivityChanged += x,
                        x => Connectivity.ConnectivityChanged -= x);

            var cancellationTokenSource =
                connectivityChanges
                    .FirstAsync(x =>
                        x.NetworkAccess != NetworkAccess.Internet ||
                        Connectivity
                            .ConnectionProfiles
                            .All(connectionProfile => connectionProfile != ConnectionProfile.WiFi))
                    .ToCancellationTokenSource();
        }
    }
}
