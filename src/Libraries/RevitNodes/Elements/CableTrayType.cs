using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RevitServices.Persistence;

namespace Revit.Elements
{
    public class CableTrayType : Element
    {
        #region Internal properties

        /// <summary>
        /// Internal reference to the Revit Element
        /// </summary>
        internal Autodesk.Revit.DB.Electrical.CableTrayType InternalCableTrayType
        {
            get;
            private set;
        }

        /// <summary>
        /// Reference to the Element
        /// </summary>
        public override Autodesk.Revit.DB.Element InternalElement
        {
            get { return InternalCableTrayType; }
        }

        #endregion

        #region Private constructors

        /// <summary>
        /// Construct from an existing Revit Element
        /// </summary>
        /// <param name="type"></param>
        private CableTrayType(Autodesk.Revit.DB.Electrical.CableTrayType type)
        {
            SafeInit(() => InitCableTrayType(type));
        }

        #endregion

        #region Helper for private constructors

        /// <summary>
        /// Initialize a CableTrayType element
        /// </summary>
        /// <param name="type"></param>
        private void InitCableTrayType(Autodesk.Revit.DB.Electrical.CableTrayType type)
        {
            InternalSetCableTrayType(type);
        }

        #endregion

        #region Private mutators

        /// <summary>
        /// Set the internal Element, ElementId, and UniqueId
        /// </summary>
        /// <param name="cableTrayType"></param>
        private void InternalSetCableTrayType(Autodesk.Revit.DB.Electrical.CableTrayType cableTrayType)
        {
            this.InternalCableTrayType = cableTrayType;
            this.InternalElementId = cableTrayType.Id;
            this.InternalUniqueId = cableTrayType.UniqueId;
        }

        #endregion

        #region Public properties
        /// <summary>
        /// Gets the name of the specified wall type
        /// </summary>
        public new string Name
        {
            get
            {
                return InternalCableTrayType.Name;
            }
        }

        #endregion

        #region Public static constructors

        /// <summary>
        /// Select a cableTrayType from the current document by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static CableTrayType ByName(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            var type = DocumentManager.Instance.ElementsOfType<Autodesk.Revit.DB.Electrical.CableTrayType>()
                .FirstOrDefault(x => x.Name == name);

            if (type == null)
            {
                throw new Exception(Properties.Resources.CableTrayTypeNotFound);
            }

            return new CableTrayType(type)
            {
                IsRevitOwned = true
            };
        }

        /// <summary>
        /// Returns a list of CableTrayType available in the current document.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<CableTrayType> Types()
        {
            return DocumentManager.Instance.ElementsOfType<Autodesk.Revit.DB.Electrical.CableTrayType>().Select(t => new CableTrayType(t) { IsRevitOwned = true });
        }

        #endregion

        #region Internal static constructors

        /// <summary>
        /// Create from an existign Revit element
        /// </summary>
        /// <param name="cableTrayType"></param>
        /// <param name="isRevitOwned"></param>
        /// <returns></returns>
        internal static CableTrayType FromExisting(Autodesk.Revit.DB.Electrical.CableTrayType cableTrayType, bool isRevitOwned)
        {
            return new CableTrayType(cableTrayType)
            {
                IsRevitOwned = isRevitOwned
            };
        }

        #endregion

        public override string ToString()
        {
            return InternalCableTrayType.Name;
        }
    }
}
