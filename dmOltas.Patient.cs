﻿//------------------------------------------------------------------------------
// This is auto-generated code.
//------------------------------------------------------------------------------
// This code was generated by Entity Developer tool using EF Core template.
// Code is generated on: 2021. 11. 17. 9:56:44
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
    public partial class Patient {

        public Patient()
        {
            this.Institutes = new List<Institute>();
            this.Vaccines = new List<Vaccine>();
            this.Physicians = new List<Physician>();
            this.Cities = new List<City>();
            OnCreated();
        }

        public virtual int id { get; set; }

        public virtual string name { get; set; }

        public virtual int city { get; set; }

        public virtual string address { get; set; }

        public virtual IList<Institute> Institutes { get; set; }

        public virtual IList<Vaccine> Vaccines { get; set; }

        public virtual IList<Physician> Physicians { get; set; }

        public virtual IList<City> Cities { get; set; }

        #region Extensibility Method Definitions

        partial void OnCreated();

        #endregion
    }

}
