using System;
using System.Collections;
using System.Collections.Generic;
using Domain;

namespace Application.Model
{
    public class Program : BaseInfo
    {
        private IEnumerable<Tuple<Discipline, ControlType>> Disciplines;
    }
}