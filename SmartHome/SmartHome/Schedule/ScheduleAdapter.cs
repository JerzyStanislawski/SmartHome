using Android.Content;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;

namespace SmartHome.Schedule
{
    public class ScheduleAdapter : ArrayAdapter<ScheduleEntity>
    {
        private readonly List<ScheduleEntity> _items;

        public ScheduleAdapter(Context context, int textViewResourceId, ScheduleEntity[] objects) 
            : base(context, textViewResourceId, objects)
        {
            _items = new List<ScheduleEntity>(objects);
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View v = convertView;
            if (v == null)
            {
                var vi = (LayoutInflater)Context.GetSystemService(Context.LayoutInflaterService);
                v = vi.Inflate(Resource.Layout.schedule_row, null);
            }
            
            var entity = _items[position];
            if (entity != null)
            {
                var roomTextView = (TextView)v.FindViewById(Resource.Id.rowRoom);
                var timeTextView = (TextView)v.FindViewById(Resource.Id.rowTime);
                
                if (roomTextView != null)
                {
                    roomTextView.Text = entity.RoomNiceName;
                }
                if (timeTextView != null)
                {
                    timeTextView.Text = entity.Time.ToString("hh\\:mm");
                }

                var icon = (ImageView)v.FindViewById(Resource.Id.rowIcon);
                if (entity.Type == ScheduleType.BLINDS)
                {
                    if (entity.OnOrUp)
                        icon.SetImageResource(Resource.Drawable.blinds_open);
                    else
                        icon.SetImageResource(Resource.Drawable.blinds_closed);
                }
                else
                {
                    if (entity.OnOrUp)
                        icon.SetImageResource(Resource.Drawable.light_on);
                    else
                        icon.SetImageResource(Resource.Drawable.light_off);
                }

            }
            return v;
        }
    }
}