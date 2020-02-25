using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNF
{
    class Trait 
    {
        public ArrayList Value { get; set; }
        public string Name { get; set; }
        public Trait(Matrix matrix, int column)
        {
            this.Name = "F" + (column+1);
            int column_length = matrix.Value.GetLength(0);
            this.Value = new ArrayList();
          
            for (int i = 0; i<column_length; i++)
            {
                this.Value.Add(matrix.Value[i, column]);
            }
        }
       
        public double PositiveNegativeRatio(Trait label)
        {
            _ = this;
            double positives = 0;
            double negatives = 0.0;
            for (int i = 0; i<this.Value.Count; i++)
            {
                if ((Convert.ToInt32(this.Value[i]) == 1)
                    && (Convert.ToInt32(label.Value[i]) == 1)) positives++;
            }

            

            for (int i = 0; i < this.Value.Count; i++)
            {
                if ((Convert.ToInt32(this.Value[i]) == 1)
                    && (Convert.ToInt32(label.Value[i]) == 0)) negatives++;
            }
            if (negatives == 0.0) negatives = 0.001;
            return (positives/negatives) ;
        }
         public ArrayList StrongNegatives(Trait Label)
        {
            _ = this;
            ArrayList list = new ArrayList();
            for (int i = 0; i < this.Value.Count; i++)
            {
                if ((Convert.ToInt32(this.Value[i]) == 0)
                    && (Convert.ToInt32(Label.Value[i]) == 0)) list.Add(i);
            }
            return list;
        }
        public ArrayList StrongPositives(Trait Label)
        {
            _ = this;
            ArrayList list = new ArrayList();
            for (int i = 0; i < this.Value.Count; i++)
            {
                if ((Convert.ToInt32(this.Value[i]) == 1)
                    && (Convert.ToInt32(Label.Value[i]) == 1)) list.Add(i);
            }
            return list;
        }
        public static ArrayList AllOnes(ArrayList resultsfromnegatives, List<Trait> list, Trait label)
        {
            ArrayList ToBeCutOffPos = new ArrayList();
            List<Trait> newlist = new List<Trait>();
            foreach(var k in resultsfromnegatives)
            {
                int index = (int)k;
                newlist.Add(list[index]);
            }
            newlist.Add(label);
            foreach(var k in newlist)
            {
                for (int i = 0; i<k.Value.Count; i++)
                {
                    var val = (int)k.Value[i];
                    if (val == 1) ToBeCutOffPos.Add(i);
                }
            }

            string z = "";
            foreach (var k in ToBeCutOffPos)
            {
                z += k;
            }
        
            var p = z.GroupBy(c => c).Select(c => new { Num = c.Key, Count = c.Count() });
            ToBeCutOffPos = new ArrayList();
            foreach (var k in p)
            {
                if (k.Count == newlist.Count) ToBeCutOffPos.Add(k.Num);
            }
            

            return ToBeCutOffPos;

        }

    }
}
