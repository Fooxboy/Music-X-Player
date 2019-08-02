using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fooxboy.MusicX.Server.Generators
{
    /// <summary>
    /// Генератор какой-либо информации
    /// </summary>
    /// <typeparam name="T">Тип возвращаемого объекта</typeparam>
    /// <typeparam name="T2">Тип принимаемого объекта</typeparam>
    public interface IGenerator<T,T2>
    {
        T Generate(T2 data);
    }
}
