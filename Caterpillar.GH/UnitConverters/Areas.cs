﻿using System;
using System.Collections.Generic;

using System.Reflection;
using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Windows.Forms;
using System.Linq;
using Grasshopper.Kernel.Parameters;
using System.ComponentModel;
using System.Diagnostics;

namespace Caterpillar.GH.UnitConverters
{
    public class Areas : CaterPillar_Base
    {

        /// <summary>
        /// Initializes a new instance of the Lengths class.
        /// </summary>
        public Areas()
          : base("Units of Area Conversion", "Areas", "Scales a Area value from a source unit to a target unit", "Maths", "Units")
        {
            SetMenuBoxes(new string[] { "Rhino", "SI", "Metric", "Imperial", "US","UK", "Nautical", "Astronomical", "Survey" });
        }

        protected List<Unit> GetUnitType(int index)
        {
            List<Unit> units = new List<Unit>();
            switch (systemNames[index])
            {
                default:
                    units = GetUnits(typeof(Caterpillar.Lengths.Custom));
                    units[0] = GetRhinoUnits(RhUnit.Length);
                    break;
                case "SI":
                    units = GetUnits(typeof(Caterpillar.Areas.SI));
                    break;
                case "Imperial":
                    units = GetUnits(typeof(Caterpillar.Areas.Imperial));
                    break;
                case "Metric":
                    units = GetUnits(typeof(Caterpillar.Areas.Metric));
                    break;
                case "US":
                    units = GetUnits(typeof(Caterpillar.Areas.US));
                    break;
                case "UK":
                    units = GetUnits(typeof(Caterpillar.Areas.UK));
                    break;
                case "Nautical":
                    units = GetUnits(typeof(Caterpillar.Areas.Nautical));
                    break;
                case "Astronomical":
                    units = GetUnits(typeof(Caterpillar.Areas.Astronomical));
                    break;
                case "Survey":
                    units = GetUnits(typeof(Caterpillar.Areas.Survey));
                    break;
            }
            return units;
        }

        /// <summary>
        /// Set Exposure level for the component.
        /// </summary>
        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.primary; }
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Value", "V", "The value in the source units", GH_ParamAccess.item, 1.0);

            pManager.AddIntegerParameter("Source Units", "S", "The source unit scale", GH_ParamAccess.item, 0);

            pManager.AddIntegerParameter("Target Units", "T", "The target unit scale", GH_ParamAccess.item, 0);

            SetInputOptions();
            SetOutputOptions();
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("Result", "R", "The converted value", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double inputValue = 1.0;
            if (!DA.GetData(0, ref inputValue)) return;
            if (!DA.GetData(1, ref inVal)) return;
            if (!DA.GetData(2, ref outVal)) return;

            if (inputIndex == 0) inVal = 0;
            if (outputIndex == 0) outVal = 0;

            DA.SetData(0, ConvertUnits(inputValue));
        }

        protected override void SetInputOptions()
        {
            base.SetInputOptions();

            UnitsIn = GetUnitType(inputIndex);

            Param_Integer paramIn = (Param_Integer)Params.Input[1];
            paramIn.ClearNamedValues();
            int i = 0;
            foreach (Unit unit in UnitsIn)
            {
                paramIn.AddNamedValue(unit.Name, i);
                i += 1;
            }
        }

        protected override void SetOutputOptions()
        {
            base.SetOutputOptions();

            UnitsOut = GetUnitType(outputIndex);

            Param_Integer paramOut = (Param_Integer)Params.Input[2];
            paramOut.ClearNamedValues();
            int i = 0;
            foreach (Unit unit in UnitsOut)
            {
                paramOut.AddNamedValue(unit.Name, i);
                i += 1;
            }
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return Properties.Resources.Area24;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("3664cbc4-1f4b-49f7-9e9a-f0d6be0d4211"); }
        }
    }
}