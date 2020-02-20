using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace SegmentadorDocumentos
{
    public partial class MainWindow
    {
        private string _routeExplorerFolder;

        private List<string> _extensions;

        public MainWindow()
        {
            _routeExplorerFolder = String.Empty;
            _extensions = new List<string>();

            InitializeComponent();

            TextBoxFileDirectory.TextChanged += (sender, args) => { _routeExplorerFolder = TextBoxFileDirectory.Text; };

            ButtonStart.Click += (sender, args) =>
            {
                GetFullExtensionsDirectory();

                DeleteDuplicatesList();

                CreateFolder();

                MoveFileOnFolder();

                // Es opcional
                ChangeNameOnList();

                var testing = _extensions;

                _extensions.Clear();
            };
        }

        private void MoveFileOnFolder()
        {
            foreach (var direcotries in Directory.GetFiles(_routeExplorerFolder))
            {
                string extensionReal = "";
                string[] arraySegmentacionDirectorios = direcotries.Split('\\');
                string documentoConExtension = arraySegmentacionDirectorios[arraySegmentacionDirectorios.Length - 1];

                if (documentoConExtension.Contains("."))
                {
                    var arrayDocumentoExtension = documentoConExtension.Split('.');
                    extensionReal = arrayDocumentoExtension[arrayDocumentoExtension.Length - 1];
                }

                foreach (var rutaCarpeta in Directory.GetDirectories(_routeExplorerFolder))
                {
                    var carpetaRaiz = rutaCarpeta.Split('\\');
                    var nombreCarpetaFinal = carpetaRaiz[carpetaRaiz.Length - 1];

                    if (extensionReal.Equals(nombreCarpetaFinal))
                    {
                        var nuevoDocumentoEnCarpeta = rutaCarpeta + "\\" + documentoConExtension;
                        System.IO.File.Move(direcotries, nuevoDocumentoEnCarpeta);
                    }
                }
            }
        }

        private void CreateFolder()
        {
            foreach (var carpeta in _extensions)
            {
                Directory.CreateDirectory(_routeExplorerFolder + "\\" + carpeta);
            }
        }

        private void ChangeNameOnList()
        {
            List<string> auxList = new List<string>();

            foreach (var extensionName in _extensions)
            {
                var extensionNewName = "Carpeta " + extensionName;
                auxList.Add(extensionNewName);
            }

            _extensions.Clear();
            _extensions = auxList;
        }

        private void DeleteDuplicatesList()
        {
            var extensionsAux = _extensions.Distinct().ToList();
            _extensions = extensionsAux;
        }

        private void GetFullExtensionsDirectory()
        {
            foreach (var direcotries in Directory.GetFiles(_routeExplorerFolder))
            {
                var extensionReal = "";
                // Segmento por la "/"
                var directorioRaiz = direcotries.Split('\\');
                // Recojo la ultima palabra
                var lastItem = directorioRaiz[directorioRaiz.Length - 1];
                // Compruebo si existe un punto que indica una extension
                if (lastItem.Contains("."))
                {
                    // Recojo la primera palabra
                    var posibleExtension = lastItem.Split('.');
                    // Recojo la ultima posible extension
                    extensionReal = posibleExtension[posibleExtension.Length - 1];
                }

                _extensions.Add(extensionReal);
            }
        }

    }
}
