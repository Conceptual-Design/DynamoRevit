using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Revit.DB;
using Revit.GeometryConversion;
using RevitServices.Persistence;
using RevitServices.Transactions;
using Point = Autodesk.DesignScript.Geometry.Point;
using DynamoServices;

namespace Revit.Elements
{
    [RegisterForTrace]
    public class CableTray : Element
    {
        #region Internal Properties

        /// <summary>
        /// Internal reference to the Revit Element
        /// </summary>
        internal Autodesk.Revit.DB.Electrical.CableTray InternalCableTray
        {
            get;
            private set;
        }

        /// <summary>
        /// Reference to the Element
        /// </summary>
        public override Autodesk.Revit.DB.Element InternalElement
        {
            get { return InternalCableTray; }
        }

        #endregion

        #region Private constructors

        /// <summary>
        /// Create from an existing Revit Element
        /// </summary>
        /// <param name="cableTray"></param>
        private CableTray(Autodesk.Revit.DB.Electrical.CableTray cableTray)
        {
            SafeInit(() => InitCableTray(cableTray));
        }

        /// <summary>
        /// Create a new instance of CableTrayType, deleting the original
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="cableTrayType"></param>
        /// <param name="level"></param>
        private CableTray(XYZ start, XYZ end, Autodesk.Revit.DB.Electrical.CableTrayType cableTrayType, Autodesk.Revit.DB.Level level)
        {
            SafeInit(() => InitCableTray(start, end, cableTrayType, level));
        }

        #endregion

        #region Helpers for private constructors

        /// <summary>
        /// Initialize a CableTray element
        /// </summary>
        /// <param name="cableTray"></param>
        private void InitCableTray(Autodesk.Revit.DB.Electrical.CableTray cableTray)
        {
            InternalSetCableTray(cableTray);
        }

        /// <summary>
        /// Initialize a CableTray element
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="cableTrayType"></param>
        /// <param name="level"></param>
        private void InitCableTray(XYZ start, XYZ end, Autodesk.Revit.DB.Electrical.CableTrayType cableTrayType, Autodesk.Revit.DB.Level level)
        {
            // This creates a new cableTray and deletes the old one
            TransactionManager.Instance.EnsureInTransaction(Document);

            //Phase 1 - Check to see if the object exists and should be rebound
            var cableTrayElem =
                ElementBinder.GetElementFromTrace<Autodesk.Revit.DB.Electrical.CableTray>(Document);

            bool successfullyUsedExistingCableTray = false;
            if (cableTrayElem != null && cableTrayElem.Location is Autodesk.Revit.DB.LocationCurve)
            {
                var cableTrayLocation = cableTrayElem.Location as Autodesk.Revit.DB.LocationCurve;
                var curve = Line.CreateBound(start, end);
                if(!CurveUtils.CurvesAreSimilar(cableTrayLocation.Curve, curve))
                    cableTrayLocation.Curve = curve;

                //Level parameter for CableTray is called Reference Level defined as RBS_START_LEVEL_PARAM
                Autodesk.Revit.DB.Parameter levelParameter =
                    cableTrayElem.get_Parameter(Autodesk.Revit.DB.BuiltInParameter.RBS_START_LEVEL_PARAM);
                Autodesk.Revit.DB.Parameter cableTrayTypeParameter =
                    cableTrayElem.get_Parameter(Autodesk.Revit.DB.BuiltInParameter.ELEM_TYPE_PARAM);
                if (levelParameter != null && levelParameter.AsElementId() != level.Id)
                    levelParameter.Set(level.Id);
                if (cableTrayTypeParameter.AsElementId() != cableTrayType.Id)
                    cableTrayTypeParameter.Set(cableTrayType.Id);
                successfullyUsedExistingCableTray = true;
            }

            var cableTray = successfullyUsedExistingCableTray ? cableTrayElem :
                     Autodesk.Revit.DB.Electrical.CableTray.Create(Document, cableTrayType.Id, start, end, level.Id);
            InternalSetCableTray(cableTray);

            TransactionManager.Instance.TransactionTaskDone();

            // delete the element stored in trace and add this new one
            ElementBinder.CleanupAndSetElementForTrace(Document, InternalCableTray);
        }

        #endregion

        #region Private mutators

        /// <summary>
        /// Set the internal Element, ElementId, and UniqueId
        /// </summary>
        /// <param name="cableTray"></param>
        private void InternalSetCableTray(Autodesk.Revit.DB.Electrical.CableTray cableTray)
        {
            InternalCableTray = cableTray;
            InternalElementId = cableTray.Id;
            InternalUniqueId = cableTray.UniqueId;
        }

        #endregion

        #region Public static constructors

        /// <summary>
        /// Create a Revit CableTray from a guiding Curve, height, Level, and CableTrayType
        /// </summary>
        /// <param name="curve"></param>
        /// <param name="height"></param>
        /// <param name="level"></param>
        /// <param name="cableTrayType"></param>
        /// <returns></returns>
        public static CableTray ByStartPointEndPoint(Point start, Point end, Level level, Element cableTrayType)
        {
            if (start == null)
            {
                throw new ArgumentNullException("start");
            }

            if (end == null)
            {
                throw new ArgumentNullException("end");
            }

            if (level == null)
            {
                throw new ArgumentNullException("level");
            }

            if (cableTrayType == null)
            {
                throw new ArgumentNullException("cableTrayType");
            }

            var type = cableTrayType.InternalElement as Autodesk.Revit.DB.Electrical.CableTrayType;
            if (null == type)
                throw new ArgumentException("The input element is not a valid CableTrayType", "cableTrayType");

            return new CableTray(start.ToXyz(), end.ToXyz(), type, level.InternalLevel);
        }

        #endregion

        #region Internal static constructors

        /// <summary>
        /// Create a Revit CableTray from an existing reference
        /// </summary>
        /// <param name="cableTray"></param>
        /// <param name="isRevitOwned"></param>
        /// <returns></returns>
        internal static CableTray FromExisting(Autodesk.Revit.DB.Electrical.CableTray cableTray, bool isRevitOwned)
        {
            return new CableTray(cableTray)
            {
                IsRevitOwned = isRevitOwned
            };
        }

        #endregion
    }
}
