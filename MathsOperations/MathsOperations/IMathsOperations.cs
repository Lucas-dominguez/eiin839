using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace MathsOperations
{
    [ServiceContract]
    public interface IMathsOperations
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "Add?a={a}&b={b}", Method ="GET", BodyStyle=WebMessageBodyStyle.Wrapped, ResponseFormat =WebMessageFormat.Json)]
        int Add(int a, int b);
        [OperationContract]
        [WebInvoke(UriTemplate = "Multiply?a={a}&b={b}", Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        int Multiply(int a, int b);
        [OperationContract]
        [WebInvoke(UriTemplate = "Subtract?a={a}&b={b}",Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        int Subtract(int a, int b);
        [OperationContract]
        [WebInvoke(UriTemplate = "Divide?a={a}&b={b}", Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        float Divide(float a, float b);

        [OperationContract]
        
        CompositeType GetDataUsingDataContract(CompositeType composite);
    }

    // Utilisez un contrat de données comme indiqué dans l'exemple ci-après pour ajouter les types composites aux opérations de service.
    // Vous pouvez ajouter des fichiers XSD au projet. Une fois le projet généré, vous pouvez utiliser directement les types de données qui y sont définis, avec l'espace de noms "MathsOperations.ContractType".
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }
}
