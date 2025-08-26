namespace VoldeNuit.Framework.Display;

using static Heart;

[Obsolete("It is not recommended to use. Please use the Camera class instead.")]
public static class ViewLegacy {

    // Legacy view variables

    public static readonly _IViewLegacy view_xview = new _IViewLegacy(typeof(View), 0);
    public static readonly _IViewLegacy view_yview = new _IViewLegacy(typeof(View), 1);

    public static readonly _IViewLegacy view_wview = new _IViewLegacy(typeof(View), 2);
    public static readonly _IViewLegacy view_hview = new _IViewLegacy(typeof(View), 3);

    public static readonly _IViewLegacy view_xport = new _IViewLegacy(typeof(Viewport), 0);
    public static readonly _IViewLegacy view_yport = new _IViewLegacy(typeof(Viewport), 0);

    public static readonly _IViewLegacy view_wport = new _IViewLegacy(typeof(Viewport), 0);
    public static readonly _IViewLegacy view_hport = new _IViewLegacy(typeof(Viewport), 0);

    public static readonly _IViewLegacy view_visible = new _IViewLegacy(typeof(Camera), 0);

    public class _IViewLegacy {

        internal _IViewLegacy(Type type, int elt) {

            this.type = type;

            this.elt = elt;
        }

        private readonly dynamic type;

        private readonly int elt;

        public dynamic? this[int i] {

            get {

                if (room_current.camera == null) { return null; }

                if (type is View) { View v = room_current.camera[i].view; 
                
                    switch (elt) {

                        case 0: { return v.x; }
                        case 1: { return v.y; }
                        case 2: { return v.width; }
                        case 3: { return v.height; }
                    }
                }

                if (type is Viewport) { Viewport vp = room_current.camera[i].viewport; 
                
                    switch (elt) {

                        case 0: { return vp.x; }
                        case 1: { return vp.y; }
                        case 2: { return vp.width; }
                        case 3: { return vp.height; }
                    }
                }

                if (type is Camera) { Camera c = room_current.camera[i]; 
                
                    switch (elt) {

                        case 0: { return c.visible; }
                    }
                }

                return null;
            }
            
            set {

                if (room_current.camera == null) { return; }

                if (type is View) { View v = room_current.camera[i].view; 
                
                    switch (elt) {

                        case 0: { v.x = value; return; }
                        case 1: { v.y = value; return; }
                        case 2: { v.width  = value; return; }
                        case 3: { v.height = value; return; }
                    }
                }

                if (type is Viewport) { Viewport vp = room_current.camera[i].viewport; 
                
                    switch (elt) {

                        case 0: { vp.x = value; return; }
                        case 1: { vp.y = value; return; }
                        case 2: { vp.width  = value; return; }
                        case 3: { vp.height = value; return; }
                    }
                }

                if (type is Camera) { Camera c = room_current.camera[i]; 
                
                    switch (elt) {

                        case 0: { c.visible = value; return; }
                    }
                }

                return;
            }
        }
    }
}