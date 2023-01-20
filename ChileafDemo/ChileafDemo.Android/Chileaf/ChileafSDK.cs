using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChileafDemo.Droid.Chileaf
{
    public class ChileafSDK
    {
        private static WeakReference<Context> _contextReference;
        public static void Init(Context context)
        {
            _contextReference = new WeakReference<Context>(context);
        }

        public static Context Context
        {
            get
            {
                _contextReference.TryGetTarget(out var context);
                return context;
            }
        }
    }
}