#region Using Directives
using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.AppCompat.App;
using Google.Android.Material.FloatingActionButton;
using Google.Android.Material.Snackbar;

using Xamarin.Essentials;
using Android.Content;
#endregion

namespace ACCompass
{
    [Activity( Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true )]
    public class MainActivity : AppCompatActivity {

        string app_name;
        Toolbar toolbar;
        AppCompatImageView compassSpinner;




        /// <summary>
        /// Activates the Compass and displays heading
        /// </summary>
        void _activateCompass ( ) {

            Compass.ReadingChanged += Compass_ReadingChanged;
            Compass.Start( SensorSpeed.Default, true );
        }

        /// <summary>
        /// Deactivates the Compass
        /// </summary>
        void _deactivateCompass ( ) {

            Compass.ReadingChanged -= Compass_ReadingChanged;
            Compass.Stop();
            
            compassSpinner.Rotation = 0f;
            toolbar.Title = $"{app_name} (Off)";
        }

        /// <summary>
        /// Toggles compass on/off
        /// </summary>
        void _toggleCompass ( ) {

            if ( !Compass.IsMonitoring ) {
                _activateCompass();
            }

            else {
                _deactivateCompass();
            }
        }



        /// <summary>
        /// Event handler for compass readings
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">compass data</param>
        private void Compass_ReadingChanged ( object sender, CompassChangedEventArgs e ) {

            var bearing = (float) ( e.Reading.HeadingMagneticNorth );

            compassSpinner.Rotation = bearing;
            toolbar.Title = $"{app_name} - Heading: { bearing.ToString( "0.0" ) }";
        }



        protected override void OnCreate ( Bundle savedInstanceState ) {

            base.OnCreate( savedInstanceState );

            Xamarin.Essentials.Platform.Init( this, savedInstanceState );
            SetContentView( Resource.Layout.activity_main );

            //! Get our toolbar reference :
            toolbar = FindViewById<Toolbar>( Resource.Id.toolbar );
            SetSupportActionBar( toolbar );

            //! Get our compass spinner reference :
            compassSpinner = FindViewById<AppCompatImageView>( Resource.Id.compassSpinner );

            //! Save the app_name string resource :
            app_name = Resources.GetString( Resource.String.app_name );
        }

        public override bool OnCreateOptionsMenu ( IMenu menu ) {
            MenuInflater.Inflate( Resource.Menu.menu_main, menu );
            return true;
        }

        public override bool OnOptionsItemSelected ( IMenuItem item ) {

            int id = item.ItemId;

            if ( id == Resource.Id.action_settings ) {

                //if ( !Compass.IsMonitoring ) {
                //    Compass.Start( SensorSpeed.Default, true );
                //}

                _toggleCompass();

                return true;
            }

            return base.OnOptionsItemSelected( item );
        }

        public override void OnRequestPermissionsResult ( int requestCode,
            string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults ) {

            Xamarin.Essentials.Platform.OnRequestPermissionsResult( requestCode, permissions, grantResults );
            base.OnRequestPermissionsResult( requestCode, permissions, grantResults );
        }


        /// <summary>
        /// App is started or reloaded
        /// </summary>
        protected override void OnStart ( ) {
            
            base.OnStart();
        }

        /// <summary>
        /// App is killed or minimized
        /// </summary>
        protected override void OnStop ( ) {

            if ( Compass.IsMonitoring )
                _deactivateCompass();

            base.OnStop(); 
        }

    }
}
