﻿//------------------------------------------------------------------------------
// This is auto-generated code.
//------------------------------------------------------------------------------
// This code was generated by Entity Developer tool using EF Core template.
// Code is generated on: 2021. 11. 20. 0:06:13
//
// Changes to this file may cause incorrect behavior and will be lost if
// the code is regenerated.
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;

namespace Oltas
{
    public partial class Vaccine {

        public Vaccine()
        {
            this.Patients = new List<Patient>();
            OnCreated();
        }

        public virtual int id { get; set; }

        public virtual string name { get; set; }

        public virtual string serial { get; set; }

        public virtual IList<Patient> Patients { get; set; }

        #region Extensibility Method Definitions

        partial void OnCreated();

        #endregion
    }

}
