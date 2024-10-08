using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointMarker.ViewModel
{

    public class DataPointFrame
    {
        public DataPointFinger left_hand;
        public DataPointFinger right_hand;
        public int frame;
    }
    public class DataPointFinger
    {
        public CentrePoint CENTRE;
        public FingerPoint THUMB_MCP;
        public FingerPoint THUMB_PIP;
        public FingerPoint THUMB_DIP;
        public FingerPoint THUMB_TIP;
        public FingerPoint FORE_MCP;
        public FingerPoint FORE_PIP;
        public FingerPoint FORE_DIP;
        public FingerPoint FORE_TIP;
        public FingerPoint MIDDLE_MCP;
        public FingerPoint MIDDLE_PIP;
        public FingerPoint MIDDLE_DIP;
        public FingerPoint MIDDLE_TIP;
        public CentrePoint RING_MCP;
        public FingerPoint RING_PIP;
        public FingerPoint RING_DIP;
        public FingerPoint RING_TIP;
        public FingerPoint LITTLE_MCP;
        public FingerPoint LITTLE_PIP;
        public FingerPoint LITTLE_DIP;
        public FingerPoint LITTLE_TIP;


    }

    public class CentrePoint
    {
        public float X;
        public float Y;
        public float Z;

        public float W=0;
        public float Wx=0;
        public float Wy=0;
        public float Wz=0;

        public float AngleCentre=0;
    }

    public class FingerPoint
    {
        public float X1;
        public float Y1;
        public float Z1;

        public float X2;
        public float Y2;
        public float Z2;

        public float X3;
        public float Y3;
        public float Z3;

        public float W3;
        public float AngleFinger=0;
    }
}
