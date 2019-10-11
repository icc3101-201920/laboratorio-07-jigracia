using Laboratorio_6_OOP_201902.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Laboratorio_6_OOP_201902.Interface
{
    interface IAttackPoints
    {

        int[] GetAttackPoints(EnumType line = EnumType.None);

    }
}
