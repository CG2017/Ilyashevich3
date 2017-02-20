using System;
using Lab_1_Color.ViewModels;

namespace Lab_1_Color.Common
{
    public class ConvertColorsService
    {
        public static void FromRgbToCmy(Color rgb, ref Color cmy)
        {
            cmy.First = 1 - rgb.First/255;
            cmy.Second = 1 - rgb.Second/255;
            cmy.Third = 1 - rgb.Third/255;
        }
        public static void FromRgbToHsv(Color rgb, ref Color hsv)
        {
            double R = rgb.First,
                G = rgb.Second,
                B = rgb.Third,
                H = 0, S, V;
            var var_R = R/255;                     //RGB from 0 to 255
            var var_G = G/255;
            var var_B = B/255;

            var var_Min = Math.Min(Math.Min(var_R, var_G), var_B);    //Min. value of RGB
            var var_Max = Math.Max(Math.Max(var_R, var_G), var_B);    //Max. value of RGB
            var del_Max = var_Max - var_Min;                        //Delta RGB value 

            V = var_Max;

            if (del_Max == 0)                     //This is a gray, no chroma...
            {
                H = 0;                                //HSV results from 0 to 1
                S = 0;
            }
            else                                    //Chromatic data...
            {
                S = del_Max/var_Max;

                var del_R = (((var_Max - var_R)/6) + (del_Max/2))/del_Max;
                var del_G = (((var_Max - var_G)/6) + (del_Max/2))/del_Max;
                var del_B = (((var_Max - var_B)/6) + (del_Max/2))/del_Max;

                if (var_R == var_Max)
                    H = del_B - del_G;
                else if (var_G == var_Max)
                    H = (1.0/3) + del_R - del_B;
                else if (var_B == var_Max)
                    H = (2.0/3) + del_G - del_R;

                if (H < 0)
                    H += 1;
                if (H > 1)
                    H -= 1;
            }
            hsv.First = H * 360;
            hsv.Second = S;
            hsv.Third = V;
        }
        public static void FromRgbToLab(Color rgb, ref Color lab)
        {
            double R = rgb.First,
                G = rgb.Second,
                B = rgb.Third;
            var var_R = R/255;        //R from 0 to 255
            var var_G = G/255;        //G from 0 to 255
            var var_B = B/255;        //B from 0 to 255

            if (var_R > 0.04045)
                var_R = Math.Pow((var_R + 0.055)/1.055, 2.4);
            else
                var_R = var_R/12.92;
            if (var_G > 0.04045)
                var_G = Math.Pow((var_G + 0.055)/1.055, 2.4);
            else var_G = var_G/12.92;
            if (var_B > 0.04045) var_B = Math.Pow((var_B + 0.055)/1.055, 2.4);
            else var_B = var_B/12.92;

            var_R = var_R*100;
            var_G = var_G*100;
            var_B = var_B*100;

            //Observer. = 2°, Illuminant = D65
            var X = var_R*0.4124 + var_G*0.3576 + var_B*0.1805;
            var Y = var_R*0.2126 + var_G*0.7152 + var_B*0.0722;
            var Z = var_R*0.0193 + var_G*0.1192 + var_B*0.9505;


            var ref_X = 95.047;
            var ref_Y = 100.000;
            var ref_Z = 108.883;
            var var_X = X/ref_X;          //ref_X =  95.047   Observer= 2°, Illuminant= D65
            var var_Y = Y/ref_Y;          //ref_Y = 100.000
            var var_Z = Z/ref_Z;         //ref_Z = 108.883

            if (var_X > 0.008856) var_X = Math.Pow(var_X, (1.0/3));
            else var_X = (7.787*var_X) + (16.0/116);
            if (var_Y > 0.008856) var_Y = Math.Pow(var_Y, (1.0/3));
            else var_Y = (7.787*var_Y) + (16.0/116);
            if (var_Z > 0.008856) var_Z = Math.Pow(var_Z, 1.0/3);
            else var_Z = (7.787*var_Z) + (16.0/116);

            lab.First = (116*var_Y) - 16;
            lab.Second = 500*(var_X - var_Y);
            lab.Third = 200*(var_Y - var_Z);
        }

        public static void FromHsvToRgb(Color hsv, ref Color rgb)
        {
            double R = 0, G = 0, B = 0,
                H = hsv.First, S = hsv.Second, V = hsv.Third;

            if (S == 0)
            {
                // находимся на оси симметрии - оттенки серого
                R = V;
                G = V;
                B = V;
            }
            else
            {
                // floor(x) возвращает наибольшее целое <= x

                int sector = (int)Math.Floor(H / 60);
                var frac = H / 60 - sector; // дробнаячасть H/60

                var T = V * (1 - S);
                var P = V * (1 - S * frac);
                var Q = V * (1 - S * (1 - frac));

                switch (sector)
                {
                    case 0: R = V; G = Q; B = T; break;
                    case 1: R = P; G = V; B = T; break;
                    case 2: R = T; G = V; B = Q; break;
                    case 3: R = T; G = P; B = V; break;
                    case 4: R = Q; G = T; B = V; break;
                    case 5: R = V; G = T; B = P; break;
                }
            }
            rgb.First = R * 255;
            rgb.Second = G * 255;
            rgb.Third = B * 255;
        }

        public static void FromCmyToRgb(Color cmy, ref Color rgb)
        {
            rgb.First = (1 - cmy.First)*255;
            rgb.Second = (1 - cmy.Second)*255;
            rgb.Third = (1 - cmy.Third)*255;
        }

        public static void FromLabToRgb(Color lab, ref Color rgb)
        {
            var var_Y = (lab.First+16)/116;
            var var_X = lab.Second / 500 + var_Y;
            var var_Z = var_Y - lab.Third /200;

            if (Math.Pow(var_Y, 3) > 0.008856) var_Y = Math.Pow(var_Y, 3);
            else var_Y = (var_Y - 16.0/116)/7.787;
            if (Math.Pow(var_X, 3) > 0.008856) var_X = Math.Pow(var_X, 3);
            else var_X = (var_X - 16.0/116)/7.787;
            if (Math.Pow(var_Z, 3) > 0.008856) var_Z = Math.Pow(var_Z, 3);
            else var_Z = (var_Z - 16.0/116)/7.787;

            var ref_X = 95.047;
            var ref_Y = 100.000;
            var ref_Z = 108.883;
            var X = ref_X*var_X;     //ref_X =  95.047     Observer= 2°, Illuminant= D65
            var Y = ref_Y*var_Y;     //ref_Y = 100.000
            var Z = ref_Z*var_Z;     //ref_Z = 108.883


            var_X = X/100;        //X from 0 to  95.047      (Observer = 2°, Illuminant = D65)
            var_Y = Y/100;        //Y from 0 to 100.000
            var_Z = Z/100;        //Z from 0 to 108.883

            var var_R = var_X*3.2406 + var_Y*-1.5372 + var_Z*-0.4986;
            var var_G = var_X*-0.9689 + var_Y*1.8758 + var_Z*0.0415;
            var var_B = var_X*0.0557 + var_Y*-0.2040 + var_Z*1.0570;

            if (var_R > 0.0031308) var_R = 1.055*Math.Pow(var_R, 1/2.4) - 0.055;
            else var_R = 12.92*var_R;
            if (var_G > 0.0031308) var_G = 1.055* Math.Pow(var_G, 1/2.4) - 0.055;
            else var_G = 12.92*var_G;
            if (var_B > 0.0031308) var_B = 1.055* Math.Pow(var_B, 1/2.4) - 0.055;
            else var_B = 12.92*var_B;

            rgb.First = var_R*255;
            rgb.Second = var_G*255;
            rgb.Third = var_B*255;
        }
    }
}
