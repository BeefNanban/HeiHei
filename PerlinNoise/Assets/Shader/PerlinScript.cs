using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinScript : MonoBehaviour {

    public class Xorshift
    {
        private uint[] _vec = new uint[4];

        public Xorshift(uint seed = 100)
        {
            for (uint i = 1; i <= _vec.Length; i++)
            {
                seed = 1812433253 * (seed ^ (seed >> 30)) + i;
                _vec[i - 1] = seed;
            }
        }

        public float Random()
        {
            uint t = _vec[0];
            uint w = _vec[3];

            _vec[0] = _vec[1];
            _vec[1] = _vec[2];
            _vec[2] = w;

            t ^= t << 11;
            t ^= t >> 8;
            w ^= w >> 19;
            w ^= t;

            _vec[3] = w;

            return w * 2.3283064365386963e-10f;
        }
    }

    public class PerlinNoise
    {
        private Xorshift _xorshift;
        private int[] _p;
        public float Frequency = 32.0f;

        public PerlinNoise(uint seed)
        {
            _xorshift = new Xorshift(seed);

            int[] p = new int[256];
            for (int i = 0; i < p.Length; i++){
                p[i] = (int)Mathf.Floor(_xorshift.Random() * 256);

            }

            int[] p2 = new int[p.Length * 2];
            for(int i = 0; i< p2.Length; i++)
            {
                p2[i] = p[i & 255];
            }
            _p = p2;
        }

        private float Fade(float t)
        {
            return t * t * t * (t * (t * 6f - 15f) + 10f);
        }
        private float Lerp(float t, float a, float b)
        {
            return a + t * (b - a);
        }
        private float Grad(int hash,float x,float y, float z)
        {
            int h = hash & 15;
            float u = (h < 8) ? x : y;
            float v = (h < 4) ? y : (h == 12 || h == 14) ? x : z;
            return ((h & 1) == 0 ? u : -u) + ((h & 2) == 0 ? v : -v);
        }

        //パーリンノイズの座標への入力
        private float Noise(float x, float y = 0, float z = 0)
        {
            int X = (int)Mathf.Floor(x) & 255;
            int Y = (int)Mathf.Floor(y) & 255;
            int Z = (int)Mathf.Floor(z) & 255;

            x -= Mathf.Floor(x);
            y -= Mathf.Floor(y);
            z -= Mathf.Floor(z);

            float u = Fade(x);
            float v = Fade(y);
            float w = Fade(z);

            int[] p = _p;

            #region ### calulate hashes from array of p ###
            int A, B, AA, AB, BA, BB, AAA, ABA, AAB, ABB, BAA, BBA, BAB, BBB;

            A = p[X + 0] + Y; AA = p[A] + Z; AB = p[A + 1] + Z;
            B = p[X + 1] + Y; BA = p[B] + Z; BB = p[B + 1] + Z;

            AAA = p[AA + 0]; ABA = p[BA + 0]; AAB = p[AB + 0]; ABB = p[BB + 0];
            BAA = p[AA + 1]; BBA = p[BA + 1]; BAB = p[AB + 1]; BBB = p[BB + 1];
            #endregion ### calulate hashes from array of p ###

            float a = Grad(AAA, x + 0, y + 0, z + 0);
            float b = Grad(ABA, x - 1, y + 0, z + 0);
            float c = Grad(AAB, x + 0, y - 1, z + 0);
            float d = Grad(ABB, x - 1, y - 1, z + 0);
            float e = Grad(BAA, x + 0, y + 0, z - 1);
            float f = Grad(BBA, x + 1, y + 0, z - 1);
            float g = Grad(BAB, x + 0, y - 1, z - 1);
            float h = Grad(BBB, x - 1, y - 1, z - 1);

            return Lerp(w, Lerp(v, Lerp(u, a, b), Lerp(u, c, d)), Lerp(v, Lerp(u, e, f), Lerp(u, g, h)));

        }
        
        public float OctaveNoise(float x, int octaves, float persistence = 0.5f)
        {
            float result = 0;
            float amp = 1.0f;
            float f = Frequency;
            float maxValue = 0;

            for(int i = 0; i < octaves; i++)
            {
                result += Noise(x*f) *amp;
                f *= 2.0f;
                maxValue += amp;
                amp *= persistence;
            }

            return result / maxValue;
        }
        public float OctaveNoise(float x, float y, int octaves, float persistence = 0.5f)
        {
            float result = 0;
            float amp = 1.0f;
            float f = Frequency;
            float maxValue = 0;
            for (int i = 0; i < octaves; i++)
            {
                result += Noise(x * f, y * f) * amp;
                f *= 2.0f;
                maxValue += amp;
                amp *= persistence;
            }

            return result / maxValue;
        }
        public float OctaveNoise(float x, float y, float z,int octaves,float persistence = 0.5f)
        {
            float result = 0;
            float amp = 1.0f;
            float f = Frequency;
            float maxValue = 0;

            for(int i = 0; i <octaves; i++)
            {
                result += Noise(x * f, y * f, z * f) * amp;
                f *= 2.0f;
                maxValue += amp;
                amp *= persistence;
            }
            return result / maxValue;
        }
    }

}
