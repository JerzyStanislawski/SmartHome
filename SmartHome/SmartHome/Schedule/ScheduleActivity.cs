using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SmartHome.Activities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartHome.Schedule
{
    public class ScheduleActivity : BasePageActivity
    {
        List<ScheduleEntity> _entities;
        ScheduleAdapter _adapter;
        ScheduleCommunication _communicationHandler;

        private int _currentId;

        public const int REQUEST_CODE_ADD_ENTITY = 0;
        public const int REQUEST_CODE_EDIT_ENTITY = 1;

        public const int RESULT_CODE_OK = 0;
        public const int RESULT_CODE_BACK = 1;
        public const int RESULT_CODE_REMOVE = 2;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_schedule);

            _communicationHandler = new ScheduleCommunication(this.ApplicationContext);
            RetrieveData();

            var list = (ListView)FindViewById(Resource.Id.scheduleList);
            list.Adapter = _adapter;

            list.Clickable = true;
            list.OnItemClickListener = new ClickListener(this);
        }

        private async void RetrieveData()
        {
            _entities = (await _communicationHandler.RetrieveData()).ToList();
            _adapter = new ScheduleAdapter(this, Resource.Layout.schedule_row, _entities.ToArray());

            _currentId = _entities.Count;
        }

        public async void NavigateOK(View _)
        {
            if (await _communicationHandler.SendData(_entities) == false)
                Toast.MakeText(this.ApplicationContext, Resource.String.send_schedule_fail, ToastLength.Short);
            else
                Finish();
        }

        public void NavigateAdd(View _)
        {
            Intent intent = new Intent(this, typeof(ScheduleEntityActivity));
            StartActivityForResult(intent, REQUEST_CODE_ADD_ENTITY);
        }

        public void NavigateClear(View _)
        {
            _entities.Clear();
            _adapter.NotifyDataSetChanged();
        }


        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (((int)resultCode) == RESULT_CODE_BACK)
                return;

            if ((int)resultCode == RESULT_CODE_REMOVE)
            {
                int id = data.GetIntExtra("id", 0);

                var entity = _entities.FirstOrDefault(x => x.Id == id);
                _entities.Remove(entity);

                _adapter.NotifyDataSetChanged();
                return;
            }

            if (resultCode == RESULT_CODE_OK)
            {
                if (requestCode == REQUEST_CODE_ADD_ENTITY)
                {
                    data.PutExtra("id", _currentId++);
                }

                ScheduleEntity entity = ScheduleEntityHelpers.CreateFromIntent(data);

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

                _adapter.NotifyDataSetChanged();
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