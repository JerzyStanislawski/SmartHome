using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.ConstraintLayout.Widget;
using SmartHome.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHome.Schedule
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", ParentActivity = typeof(MainActivity))]
    public class ScheduleActivity : BasePageActivity
    {
        List<ScheduleEntity> _entities;
        ScheduleAdapter _adapter;
        ScheduleCommunication _communicationHandler;

        private int _currentId;

        public const int REQUEST_CODE_ADD_ENTITY = 0;
        public const int REQUEST_CODE_EDIT_ENTITY = 1;

        public const int RESULT_CODE_REMOVE = 2;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_schedule);

            EnableButtons(false);

            _communicationHandler = new ScheduleCommunication(this.ApplicationContext);
            Task.Run(async () => await RetrieveData())
                .ContinueWith(task =>
                {
                    RunOnUiThread(() =>
                    {
                        if (task.IsCompletedSuccessfully)
                        {
                            var list = (ListView)FindViewById(Resource.Id.scheduleList);
                            list.Adapter = _adapter;

                            list.Clickable = true;
                            list.OnItemClickListener = new ClickListener(this);

                            EnableButtons(true);
                        }
                        else
                            Toast.MakeText(ApplicationContext, Resource.String.connection_failure_message, ToastLength.Short).Show();
                    });
                });
        }

        private async Task RetrieveData()
        {
            _entities = (await _communicationHandler.RetrieveData()).ToList();
            _adapter = new ScheduleAdapter(this, Resource.Layout.schedule_row, _entities);

            _currentId = _entities.Count;
        }

        [Java.Interop.Export("NavigateOK")]
        public async void NavigateOK(View _)
        {
            if (await _communicationHandler.SendData(_entities) == false)
                Toast.MakeText(this.ApplicationContext, Resource.String.send_schedule_fail, ToastLength.Short);
            else
                Finish();
        }

        [Java.Interop.Export("NavigateAdd")]
        public void NavigateAdd(View _)
        {
            Intent intent = new Intent(this, typeof(ScheduleEntityActivity));
            StartActivityForResult(intent, REQUEST_CODE_ADD_ENTITY);
        }

        [Java.Interop.Export("NavigateClear")]
        public void NavigateClear(View _)
        {
            _entities.Clear();
            _adapter.NotifyDataSetChanged();
        }


        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (resultCode == Result.Canceled)
                return;

            if ((int)resultCode == RESULT_CODE_REMOVE)
            {
                int id = data.GetIntExtra("id", 0);

                var entity = _entities.FirstOrDefault(x => x.Id == id);
                _entities.Remove(entity);
            }

            if (resultCode == Result.Ok)
            {
                if (requestCode == REQUEST_CODE_ADD_ENTITY)
                {
                    data.PutExtra("id", _currentId++);
                }

                var entity = ScheduleEntityHelpers.CreateFromIntent(data);

                if (requestCode == REQUEST_CODE_ADD_ENTITY)
                {
                    _entities.Add(entity);
                }
                else if (requestCode == REQUEST_CODE_EDIT_ENTITY)
                {
                    var entityToReplace = _entities.FirstOrDefault(x => x.Id == entity.Id);
                    int index = _entities.IndexOf(entityToReplace);
                    _entities[index] = entity;
                }
            }

            _adapter.Clear();
            _adapter.AddAll(_entities);
            _adapter.NotifyDataSetChanged();
        }

        private void EnableButtons(bool enable)
        {
            var root = FindViewById<ConstraintLayout>(Resource.Id.scheduleRoot);
            for (var i = 0; i < root.ChildCount; i++)
            {
                var child = root.GetChildAt(i);
                if (child is Button button)
                {
                    button.Enabled = enable;
                }
            }
        }

        private class ClickListener : Java.Lang.Object, AdapterView.IOnItemClickListener
        {
            private readonly ScheduleActivity _scheduleActivity;

            public ClickListener(ScheduleActivity scheduleActivity)
            {
                _scheduleActivity = scheduleActivity;
            }

            public void OnItemClick(AdapterView adapterView, View view, int position, long id)
            {
                var entity = adapterView.GetItemAtPosition(position).JavaCast<ScheduleEntity>();

                var intent = new Intent(_scheduleActivity, typeof(ScheduleEntityActivity));
                ScheduleEntityHelpers.FillIntent(entity, intent);
                _scheduleActivity.StartActivityForResult(intent, REQUEST_CODE_EDIT_ENTITY);
            }
        }
    }
}