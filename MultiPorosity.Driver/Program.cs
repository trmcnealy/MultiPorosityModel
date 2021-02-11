using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

using Engineering.DataSource;
using Engineering.DataSource.OilGas;

using Kokkos;

using MultiPorosity.Models;
using MultiPorosity.Services;

using NumericalMethods.DataStorage;

using OilGas.Data;
using OilGas.Data.RRC.Texas;

using Engineering.DataSource.Tools;

using HDF.PInvoke;

using Unsafe = UnManaged.Unsafe;

namespace MultiPorosity.Driver
{
    internal class Program
    {
        private static readonly ApiNumber[] TestWells =
        {
            new ApiNumber("42-025-33724-00-00"), new ApiNumber("42-297-35266-00-00"), new ApiNumber("42-123-32806-00-00"), new ApiNumber("42-123-32546-00-00"),
            new ApiNumber("42-123-32848-00-00"), new ApiNumber("42-297-35087-00-00"), new ApiNumber("42-297-35343-00-00"), new ApiNumber("42-297-34747-00-00"),
            new ApiNumber("42-255-35575-00-00"), new ApiNumber("42-123-32998-00-00"), new ApiNumber("42-123-32462-00-00"), new ApiNumber("42-297-34879-00-00"),
            new ApiNumber("42-297-35350-00-00"), new ApiNumber("42-123-32786-00-00"), new ApiNumber("42-255-35843-00-00"), new ApiNumber("42-255-32990-00-00"),
            new ApiNumber("42-311-35071-00-00"), new ApiNumber("42-123-34330-00-00"), new ApiNumber("42-123-34413-00-00"), new ApiNumber("42-297-34814-00-00"),
            new ApiNumber("42-297-35859-00-00"), new ApiNumber("42-123-32679-00-00"), new ApiNumber("42-123-34607-00-00"), new ApiNumber("42-123-33117-00-00"),
            new ApiNumber("42-123-32357-00-00"), new ApiNumber("42-123-32892-00-00"), new ApiNumber("42-123-34306-00-00"), new ApiNumber("42-123-32813-00-00"),
            new ApiNumber("42-123-32816-00-00"), new ApiNumber("42-255-31931-00-00"), new ApiNumber("42-123-32634-00-00"), new ApiNumber("42-255-32463-00-00"),
            new ApiNumber("42-123-32596-00-00"), new ApiNumber("42-297-35193-00-00"), new ApiNumber("42-123-34049-00-00"), new ApiNumber("42-297-35284-00-00"),
            new ApiNumber("42-255-32457-00-00"), new ApiNumber("42-123-34639-00-00"), new ApiNumber("42-297-35367-00-00"), new ApiNumber("42-123-34416-00-00"),
            new ApiNumber("42-123-34604-00-00"), new ApiNumber("42-297-35158-00-00"), new ApiNumber("42-123-32789-00-00"), new ApiNumber("42-123-32784-00-00"),
            new ApiNumber("42-123-34318-00-00"), new ApiNumber("42-123-32762-00-00"), new ApiNumber("42-297-35097-00-00"), new ApiNumber("42-255-32980-00-00"),
            new ApiNumber("42-255-33014-00-00"), new ApiNumber("42-123-32739-00-00"), new ApiNumber("42-297-35298-00-00"), new ApiNumber("42-297-34861-00-00"),
            new ApiNumber("42-255-31719-00-00"), new ApiNumber("42-255-31700-00-00"), new ApiNumber("42-255-31691-00-00"), new ApiNumber("42-123-32302-00-00"),
            new ApiNumber("42-255-31724-00-00"), new ApiNumber("42-025-33726-00-00"), new ApiNumber("42-255-31714-00-00"), new ApiNumber("42-123-32722-00-00"),
            new ApiNumber("42-255-31657-00-00"), new ApiNumber("42-123-32362-00-00"), new ApiNumber("42-311-35254-00-00"), new ApiNumber("42-123-34073-00-00"),
            new ApiNumber("42-255-31756-00-00"), new ApiNumber("42-297-34921-00-00"), new ApiNumber("42-123-34325-00-00"), new ApiNumber("42-123-34442-00-00"),
            new ApiNumber("42-123-34629-00-00"), new ApiNumber("42-297-35806-00-00"), new ApiNumber("42-123-34605-00-00"), new ApiNumber("42-123-34329-00-00"),
            new ApiNumber("42-123-34302-00-00"), new ApiNumber("42-123-34412-00-00"), new ApiNumber("42-255-35395-00-00"), new ApiNumber("42-123-32658-00-00"),
            new ApiNumber("42-123-32551-00-00"), new ApiNumber("42-123-32501-00-00"), new ApiNumber("42-123-34144-00-00"), new ApiNumber("42-123-32306-00-00"),
            new ApiNumber("42-123-32615-00-00"), new ApiNumber("42-123-34597-00-00"), new ApiNumber("42-123-34826-00-00"), new ApiNumber("42-123-32320-00-00"),
            new ApiNumber("42-123-34598-00-00"), new ApiNumber("42-123-32450-00-00"), new ApiNumber("42-123-34535-00-00"), new ApiNumber("42-123-34283-00-00"),
            new ApiNumber("42-123-32986-00-00"), new ApiNumber("42-123-32443-00-00"), new ApiNumber("42-123-34716-00-00"), new ApiNumber("42-123-32494-00-00"),
            new ApiNumber("42-123-32338-00-00"), new ApiNumber("42-123-34282-00-00"), new ApiNumber("42-123-32903-00-00"), new ApiNumber("42-123-32316-00-00"),
            new ApiNumber("42-123-32578-00-00"), new ApiNumber("42-123-32506-00-00"), new ApiNumber("42-123-34236-00-00"), new ApiNumber("42-123-32481-00-00"),
            new ApiNumber("42-123-32902-00-00"), new ApiNumber("42-123-33072-00-00"), new ApiNumber("42-123-33266-00-00"), new ApiNumber("42-123-33062-00-00"),
            new ApiNumber("42-123-32365-00-00"), new ApiNumber("42-123-32622-00-00"), new ApiNumber("42-123-34600-00-00"), new ApiNumber("42-255-32699-00-00"),
            new ApiNumber("42-123-33640-00-00"), new ApiNumber("42-123-33017-00-00"), new ApiNumber("42-123-34275-00-00"), new ApiNumber("42-123-32304-00-00"),
            new ApiNumber("42-123-34719-00-00"), new ApiNumber("42-123-34555-00-00"), new ApiNumber("42-123-34601-00-00"), new ApiNumber("42-123-34588-00-00"),
            new ApiNumber("42-123-34596-00-00"), new ApiNumber("42-123-32483-00-00"), new ApiNumber("42-123-34599-00-00"), new ApiNumber("42-123-34584-00-00"),
            new ApiNumber("42-123-33164-00-00"), new ApiNumber("42-123-32999-00-00"), new ApiNumber("42-123-33564-00-00"), new ApiNumber("42-123-32475-00-00"),
            new ApiNumber("42-123-34556-00-00"), new ApiNumber("42-123-32674-00-00"), new ApiNumber("42-123-32823-00-00"), new ApiNumber("42-123-32301-00-00"),
            new ApiNumber("42-123-34536-00-00"), new ApiNumber("42-123-33102-00-00"), new ApiNumber("42-123-32642-00-00"), new ApiNumber("42-123-33101-00-00"),
            new ApiNumber("42-123-33090-00-00"), new ApiNumber("42-123-33015-00-00"), new ApiNumber("42-123-34035-00-00"), new ApiNumber("42-123-32451-00-00"),
            new ApiNumber("42-123-32910-00-00"), new ApiNumber("42-123-34714-00-00"), new ApiNumber("42-123-33299-00-00"), new ApiNumber("42-123-34585-00-00"),
            new ApiNumber("42-123-34742-00-00"), new ApiNumber("42-123-32911-00-00"), new ApiNumber("42-255-33623-00-00"), new ApiNumber("42-123-33710-00-00"),
            new ApiNumber("42-255-33493-00-00"), new ApiNumber("42-297-35326-00-00"), new ApiNumber("42-123-34156-00-00"), new ApiNumber("42-297-35792-00-00"),
            new ApiNumber("42-297-35027-00-00"), new ApiNumber("42-255-32163-00-00"), new ApiNumber("42-255-33202-00-00"), new ApiNumber("42-255-32300-00-00"),
            new ApiNumber("42-123-33246-00-00"), new ApiNumber("42-123-32295-00-00"), new ApiNumber("42-255-33468-00-00"), new ApiNumber("42-123-33716-00-00"),
            new ApiNumber("42-297-35934-00-00"), new ApiNumber("42-255-33561-00-00"), new ApiNumber("42-255-34628-00-00"), new ApiNumber("42-297-35376-00-00"),
            new ApiNumber("42-297-35308-00-00"), new ApiNumber("42-123-33186-00-00"), new ApiNumber("42-255-32153-00-00"), new ApiNumber("42-297-35416-00-00"),
            new ApiNumber("42-123-33692-00-00"), new ApiNumber("42-255-35335-00-00"), new ApiNumber("42-297-35481-00-00"), new ApiNumber("42-123-33501-00-00"),
            new ApiNumber("42-123-33572-00-00"), new ApiNumber("42-123-33586-00-00"), new ApiNumber("42-123-33712-00-00"), new ApiNumber("42-255-34605-00-00"),
            new ApiNumber("42-255-34342-00-00"), new ApiNumber("42-123-33960-00-00"), new ApiNumber("42-123-33543-00-00"), new ApiNumber("42-123-34063-00-00"),
            new ApiNumber("42-255-34611-00-00"), new ApiNumber("42-255-31948-00-00"), new ApiNumber("42-123-33986-00-00"), new ApiNumber("42-297-35484-00-00"),
            new ApiNumber("42-255-34607-00-00"), new ApiNumber("42-123-32951-00-00"), new ApiNumber("42-255-33836-00-00"), new ApiNumber("42-123-33697-00-00"),
            new ApiNumber("42-297-34901-00-00"), new ApiNumber("42-255-33007-00-00"), new ApiNumber("42-255-34457-00-00"), new ApiNumber("42-123-32590-00-00"),
            new ApiNumber("42-255-34352-00-00"), new ApiNumber("42-123-32436-00-00"), new ApiNumber("42-123-32647-00-00"), new ApiNumber("42-255-32225-00-00"),
            new ApiNumber("42-255-34216-00-00"), new ApiNumber("42-123-34115-00-00"), new ApiNumber("42-123-34164-00-00"), new ApiNumber("42-297-35781-00-00"),
            new ApiNumber("42-123-34155-00-00"), new ApiNumber("42-297-35375-00-00"), new ApiNumber("42-123-33987-00-00"), new ApiNumber("42-123-33961-00-00"),
            new ApiNumber("42-123-33483-00-00"), new ApiNumber("42-123-32932-00-00"), new ApiNumber("42-255-35333-00-00"), new ApiNumber("42-297-35556-00-00"),
            new ApiNumber("42-255-35329-00-00"), new ApiNumber("42-297-35065-00-00"), new ApiNumber("42-255-33724-00-00"), new ApiNumber("42-297-35477-00-00"),
            new ApiNumber("42-123-33247-00-00"), new ApiNumber("42-297-35030-00-00"), new ApiNumber("42-297-35129-00-00"), new ApiNumber("42-255-32247-00-00"),
            new ApiNumber("42-123-32718-00-00"), new ApiNumber("42-297-35156-00-00"), new ApiNumber("42-255-33342-00-00"), new ApiNumber("42-297-35476-00-00"),
            new ApiNumber("42-297-35306-00-00"), new ApiNumber("42-123-33821-00-00"), new ApiNumber("42-255-34453-00-00"), new ApiNumber("42-123-33698-00-00"),
            new ApiNumber("42-297-35602-00-00"), new ApiNumber("42-255-34214-00-00"), new ApiNumber("42-255-33590-00-00"), new ApiNumber("42-297-35324-00-00"),
            new ApiNumber("42-123-33544-00-00"), new ApiNumber("42-297-35068-00-00"), new ApiNumber("42-255-33492-00-00"), new ApiNumber("42-123-33219-00-00"),
            new ApiNumber("42-297-34846-00-00"), new ApiNumber("42-255-33488-00-00"), new ApiNumber("42-123-33963-00-00"), new ApiNumber("42-255-33489-00-00"),
            new ApiNumber("42-297-35325-00-00"), new ApiNumber("42-123-33346-00-00"), new ApiNumber("42-123-33500-00-00"), new ApiNumber("42-297-35480-00-00"),
            new ApiNumber("42-297-35131-00-00"), new ApiNumber("42-255-33619-00-00"), new ApiNumber("42-297-35113-00-00"), new ApiNumber("42-255-33680-00-00"),
            new ApiNumber("42-123-33964-00-00"), new ApiNumber("42-123-33569-00-00"), new ApiNumber("42-255-34649-00-00"), new ApiNumber("42-123-33485-00-00"),
            new ApiNumber("42-297-35307-00-00"), new ApiNumber("42-123-32949-00-00"), new ApiNumber("42-123-33491-00-00"), new ApiNumber("42-123-33528-00-00"),
            new ApiNumber("42-255-33491-00-00"), new ApiNumber("42-255-33591-00-00"), new ApiNumber("42-255-33566-00-00"), new ApiNumber("42-123-33492-00-00"),
            new ApiNumber("42-255-33722-00-00"), new ApiNumber("42-255-34646-00-00"), new ApiNumber("42-297-35374-00-00"), new ApiNumber("42-255-35330-00-00"),
            new ApiNumber("42-297-35494-00-00"), new ApiNumber("42-123-33222-00-00"), new ApiNumber("42-297-35555-00-00"), new ApiNumber("42-255-33834-00-00"),
            new ApiNumber("42-255-34459-00-00"), new ApiNumber("42-123-33820-00-00"), new ApiNumber("42-123-34198-00-00"), new ApiNumber("42-255-34629-00-00"),
            new ApiNumber("42-255-34366-00-00"), new ApiNumber("42-255-34346-00-00"), new ApiNumber("42-123-34064-00-00"), new ApiNumber("42-255-34632-00-00"),
            new ApiNumber("42-123-33694-00-00"), new ApiNumber("42-123-33711-00-00"), new ApiNumber("42-255-33824-00-00"), new ApiNumber("42-255-33213-00-00"),
            new ApiNumber("42-123-34197-00-00"), new ApiNumber("42-255-36331-00-00"), new ApiNumber("42-255-35950-00-00"), new ApiNumber("42-255-35947-00-00"),
            new ApiNumber("42-255-35933-00-00"), new ApiNumber("42-123-32580-00-00"), new ApiNumber("42-123-33373-00-00"), new ApiNumber("42-123-33048-00-00"),
            new ApiNumber("42-123-33050-00-00"), new ApiNumber("42-123-34758-00-00"), new ApiNumber("42-123-33140-00-00"), new ApiNumber("42-123-33238-00-00"),
            new ApiNumber("42-123-33237-00-00"), new ApiNumber("42-123-33374-00-00"), new ApiNumber("42-123-33319-00-00"), new ApiNumber("42-123-34757-00-00"),
            new ApiNumber("42-123-33150-00-00"), new ApiNumber("42-123-33141-00-00"), new ApiNumber("42-123-33239-00-00"), new ApiNumber("42-123-33049-00-00"),
            new ApiNumber("42-255-34902-00-00"), new ApiNumber("42-255-34951-00-00"), new ApiNumber("42-255-33605-00-00"), new ApiNumber("42-255-35155-00-00"),
            new ApiNumber("42-255-35285-00-00"), new ApiNumber("42-255-35627-00-00"), new ApiNumber("42-255-32591-00-00"), new ApiNumber("42-255-32512-00-00"),
            new ApiNumber("42-255-34168-00-00"), new ApiNumber("42-255-33082-00-00"), new ApiNumber("42-123-33217-00-00"), new ApiNumber("42-255-34227-00-00"),
            new ApiNumber("42-255-32571-00-00"), new ApiNumber("42-255-34657-00-00"), new ApiNumber("42-255-33318-00-00"), new ApiNumber("42-255-34900-00-00"),
            new ApiNumber("42-255-34955-00-00"), new ApiNumber("42-297-35094-00-00"), new ApiNumber("42-255-32324-00-00"), new ApiNumber("42-255-33641-00-00"),
            new ApiNumber("42-297-35959-00-00"), new ApiNumber("42-123-32781-00-00"), new ApiNumber("42-255-34713-00-00"), new ApiNumber("42-255-31721-00-00"),
            new ApiNumber("42-255-33970-00-00"), new ApiNumber("42-255-36084-00-00"), new ApiNumber("42-255-34898-00-00"), new ApiNumber("42-255-34355-00-00"),
            new ApiNumber("42-255-35135-00-00"), new ApiNumber("42-255-33313-00-00"), new ApiNumber("42-123-32835-00-00"), new ApiNumber("42-255-35628-00-00"),
            new ApiNumber("42-255-33033-00-00"), new ApiNumber("42-255-32337-00-00"), new ApiNumber("42-255-33320-00-00"), new ApiNumber("42-255-32464-00-00"),
            new ApiNumber("42-255-35133-00-00"), new ApiNumber("42-255-34112-00-00"), new ApiNumber("42-255-34279-00-00"), new ApiNumber("42-255-33309-00-00"),
            new ApiNumber("42-255-33545-00-00"), new ApiNumber("42-255-33729-00-00"), new ApiNumber("42-297-35814-00-00"), new ApiNumber("42-255-32531-00-00"),
            new ApiNumber("42-297-35697-00-00"), new ApiNumber("42-255-32755-00-00"), new ApiNumber("42-255-34297-00-00"), new ApiNumber("42-255-35156-00-00"),
            new ApiNumber("42-255-34329-00-00"), new ApiNumber("42-255-34826-00-00"), new ApiNumber("42-255-35129-00-00"), new ApiNumber("42-255-34088-00-00"),
            new ApiNumber("42-255-34824-00-00"), new ApiNumber("42-255-35192-00-00"), new ApiNumber("42-255-34111-00-00"), new ApiNumber("42-255-33547-00-00"),
            new ApiNumber("42-297-35669-00-00"), new ApiNumber("42-255-32817-00-00"), new ApiNumber("42-255-35260-00-00"), new ApiNumber("42-297-35614-00-00"),
            new ApiNumber("42-255-33661-00-00"), new ApiNumber("42-297-35741-00-00"), new ApiNumber("42-255-34089-00-00"), new ApiNumber("42-255-34838-00-00"),
            new ApiNumber("42-255-34822-00-00"), new ApiNumber("42-255-34819-00-00"), new ApiNumber("42-255-33505-00-00"), new ApiNumber("42-255-32811-00-00"),
            new ApiNumber("42-255-32820-00-00"), new ApiNumber("42-255-34686-00-00"), new ApiNumber("42-255-32856-00-00"), new ApiNumber("42-123-32834-00-00"),
            new ApiNumber("42-255-32397-00-00"), new ApiNumber("42-123-32690-00-00"), new ApiNumber("42-255-34844-00-00"), new ApiNumber("42-255-34952-00-00"),
            new ApiNumber("42-255-32815-00-00"), new ApiNumber("42-255-32835-00-00"), new ApiNumber("42-255-34584-00-00"), new ApiNumber("42-255-32442-00-00"),
            new ApiNumber("42-255-36083-00-00"), new ApiNumber("42-255-35545-00-00"), new ApiNumber("42-255-33539-00-00"), new ApiNumber("42-297-35945-00-00"),
            new ApiNumber("42-255-35145-00-00"), new ApiNumber("42-255-33932-00-00"), new ApiNumber("42-255-36009-00-00"), new ApiNumber("42-297-35191-00-00"),
            new ApiNumber("42-297-35912-00-00"), new ApiNumber("42-255-33968-00-00"), new ApiNumber("42-255-34330-00-00"), new ApiNumber("42-255-32345-00-00"),
            new ApiNumber("42-255-32406-00-00"), new ApiNumber("42-255-32340-00-00"), new ApiNumber("42-255-35638-00-00"), new ApiNumber("42-297-35672-00-00"),
            new ApiNumber("42-255-33816-00-00"), new ApiNumber("42-255-33568-00-00"), new ApiNumber("42-255-33960-00-00"), new ApiNumber("42-255-33958-00-00"),
            new ApiNumber("42-255-32534-00-00"), new ApiNumber("42-297-35913-00-00"), new ApiNumber("42-297-35190-00-00"), new ApiNumber("42-255-33798-00-00"),
            new ApiNumber("42-297-35870-00-00"), new ApiNumber("42-255-32575-00-00"), new ApiNumber("42-255-32533-00-00"), new ApiNumber("42-255-34615-00-00"),
            new ApiNumber("42-297-35813-00-00"), new ApiNumber("42-255-33964-00-00"), new ApiNumber("42-255-33971-00-00"), new ApiNumber("42-255-35264-00-00"),
            new ApiNumber("42-255-33544-00-00"), new ApiNumber("42-255-33659-00-00"), new ApiNumber("42-255-33502-00-00"), new ApiNumber("42-255-32579-00-00"),
            new ApiNumber("42-255-34663-00-00"), new ApiNumber("42-255-33671-00-00"), new ApiNumber("42-255-34770-00-00"), new ApiNumber("42-255-34027-00-00"),
            new ApiNumber("42-255-34145-00-00"), new ApiNumber("42-255-34028-00-00"), new ApiNumber("42-255-35544-00-00"), new ApiNumber("42-297-35867-00-00"),
            new ApiNumber("42-255-32281-00-00"), new ApiNumber("42-297-35699-00-00"), new ApiNumber("42-255-34498-00-00"), new ApiNumber("42-255-32239-00-00"),
            new ApiNumber("42-255-34328-00-00"), new ApiNumber("42-255-34956-00-00"), new ApiNumber("42-255-34617-00-00"), new ApiNumber("42-123-33003-00-00"),
            new ApiNumber("42-255-34841-00-00"), new ApiNumber("42-255-34579-00-00"), new ApiNumber("42-255-33538-00-00"), new ApiNumber("42-255-35194-00-00"),
            new ApiNumber("42-255-34839-00-00"), new ApiNumber("42-297-35868-00-00"), new ApiNumber("42-255-32546-00-00"), new ApiNumber("42-255-35769-00-00"),
            new ApiNumber("42-255-31823-00-00"), new ApiNumber("42-255-35467-00-00"), new ApiNumber("42-255-34760-00-00"), new ApiNumber("42-255-31883-00-00"),
            new ApiNumber("42-255-32172-00-00"), new ApiNumber("42-255-36270-00-00"), new ApiNumber("42-255-35112-00-00"), new ApiNumber("42-255-35770-00-00"),
            new ApiNumber("42-255-33011-00-00"), new ApiNumber("42-255-35733-00-00"), new ApiNumber("42-255-35772-00-00"), new ApiNumber("42-255-31935-00-00"),
            new ApiNumber("42-255-32298-00-00"), new ApiNumber("42-255-32513-00-00"), new ApiNumber("42-255-35106-00-00"), new ApiNumber("42-255-35982-00-00"),
            new ApiNumber("42-255-35865-00-00"), new ApiNumber("42-255-35345-00-00"), new ApiNumber("42-285-33598-00-00"), new ApiNumber("42-255-35003-00-00"),
            new ApiNumber("42-123-32568-00-00"), new ApiNumber("42-255-31761-00-00"), new ApiNumber("42-025-33717-00-00"), new ApiNumber("42-123-32333-00-00"),
            new ApiNumber("42-255-31777-00-00"), new ApiNumber("42-255-31650-00-00"), new ApiNumber("42-255-34756-00-00"), new ApiNumber("42-255-35791-00-00"),
            new ApiNumber("42-255-35651-00-00"), new ApiNumber("42-255-31808-00-00"), new ApiNumber("42-255-32320-00-00"), new ApiNumber("42-255-32948-00-00"),
            new ApiNumber("42-255-35005-00-00"), new ApiNumber("42-123-32641-00-00"), new ApiNumber("42-255-32102-00-00"), new ApiNumber("42-123-32452-00-00"),
            new ApiNumber("42-255-32171-00-00"), new ApiNumber("42-297-35020-00-00"), new ApiNumber("42-123-32528-00-00"), new ApiNumber("42-123-34590-00-00"),
            new ApiNumber("42-297-34970-00-00"), new ApiNumber("42-255-34971-00-00"), new ApiNumber("42-255-34972-00-00"), new ApiNumber("42-255-35883-00-00"),
            new ApiNumber("42-255-35556-00-00"), new ApiNumber("42-255-32200-00-00"), new ApiNumber("42-123-32413-00-00"), new ApiNumber("42-255-35886-00-00"),
            new ApiNumber("42-255-35911-00-00"), new ApiNumber("42-255-32279-00-00"), new ApiNumber("42-123-32317-00-00"), new ApiNumber("42-255-32264-00-00"),
            new ApiNumber("42-255-31622-00-00"), new ApiNumber("42-255-32254-00-00"), new ApiNumber("42-255-32967-00-00"), new ApiNumber("42-123-32315-00-00"),
            new ApiNumber("42-123-32591-00-00"), new ApiNumber("42-255-31806-00-00"), new ApiNumber("42-255-35557-00-00"), new ApiNumber("42-255-34920-00-00"),
            new ApiNumber("42-255-34919-00-00"), new ApiNumber("42-255-35885-00-00"), new ApiNumber("42-255-34970-00-00"), new ApiNumber("42-255-34758-00-00"),
            new ApiNumber("42-255-35646-00-00"), new ApiNumber("42-123-34472-00-00"), new ApiNumber("42-123-33574-00-00"), new ApiNumber("42-123-33393-00-00"),
            new ApiNumber("42-123-34578-00-00"), new ApiNumber("42-123-33530-00-00"), new ApiNumber("42-123-33498-00-00"), new ApiNumber("42-123-34579-00-00"),
            new ApiNumber("42-123-33000-00-00"), new ApiNumber("42-123-33224-00-00"), new ApiNumber("42-123-33754-00-00"), new ApiNumber("42-123-33545-00-00"),
            new ApiNumber("42-297-35390-00-00"), new ApiNumber("42-297-35633-00-00"), new ApiNumber("42-297-35255-00-00"), new ApiNumber("42-297-35520-00-00"),
            new ApiNumber("42-297-35345-00-00"), new ApiNumber("42-297-35523-00-00"), new ApiNumber("42-297-35315-00-00")
        };

        public static T QueryInterface<T>(object obj)
        {
            if(obj == null || !Marshal.IsComObject(obj))
            {
                return (T)obj;
            }

            Guid gUID = typeof(T).GUID;

            if(Marshal.QueryInterface(Marshal.GetIUnknownForObject(obj), ref gUID, out IntPtr zero) >= 0 && zero != IntPtr.Zero)
            {
                return (T)Marshal.GetUniqueObjectForIUnknown(zero);
            }

            return default;
        }

        //private readonly Random _rand = new Random();


        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        private static unsafe void* AddressOf<T>(ref T value) where T : unmanaged
        {
            fixed (T* pn = &value)
            {
                return pn;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        private static unsafe void TestAddressOf<T>(ref T value) where T : unmanaged
        {
            void* addr = AddressOf<T>(ref value);

            Console.WriteLine($"addr 0x{((ulong)addr):X16}");

            addr = Unsafe.AddressOf<T>(ref value);

            Console.WriteLine($"addr 0x{((ulong)addr):X16}");

        }


        private static ulong value = 0xFF00FF00;


        [STAThread]
        private static void Main(string[] args)
        {

            
            TestAddressOf(ref value);


            //CreateADataset();

            //TestRelativePermeability();
            //TestMPM();

#if DEBUG
            Console.WriteLine("press any key to exit.");
            Console.ReadKey();
#endif
        }


        private static void CreateADataset()
        {
            unsafe
            {
                string FILE_NAME    = "h5tutr_dset.h5";
                string DATASET_NAME = "dset";




                long    file_id;
                long    dataset_id;
                long    dataspace_id;
                ulong[] dims = new ulong[2];
                int     status;


                int[,] dset_data =new int[4,6];

                /* Initialize the dataset. */
                for (int i = 0; i < 4; i++)
                {
                    for(int j = 0; j < 6; j++)
                    {
                        dset_data[i, j] = i * 6 + j + 1;
                    }
                }

                /* Create a new file using default properties. */
                file_id = H5F.create(FILE_NAME, H5F.ACC_RDWR, H5P.DEFAULT, H5P.DEFAULT);

                /* Create the data space for the dataset. */
                dims[0]      = 4;
                dims[1]      = 6;
                dataspace_id = H5S.create_simple(2, dims, null);

                /* Create the dataset. */
                dataset_id = H5D.create(file_id, "/dset", H5T.STD_I32BE, dataspace_id, H5P.DEFAULT, H5P.DEFAULT, H5P.DEFAULT);


                status = H5D.write(dataset_id, H5T.NATIVE_INT, H5S.ALL, H5S.ALL, H5P.DEFAULT, (IntPtr)Unsafe.AsPointer(ref dset_data[0, 0]));

                status = H5D.read(dataset_id, H5T.NATIVE_INT, H5S.ALL, H5S.ALL, H5P.DEFAULT, (IntPtr)Unsafe.AsPointer(ref dset_data[0, 0]));




                /* End access to the dataset and release resources used by it. */
                status = H5D.close(dataset_id);

                /* Terminate access to the data space. */
                status = H5S.close(dataspace_id);

                /* Close the file. */
                status = H5F.close(file_id);
            }
        }


        private static void TestRelativePermeability()
        {
            InitArguments arguments = new InitArguments(8, -1, 0, true);

            using(ScopeGuard.Get(arguments))
            {
                double saturation_water_connate  = 0.13;
                double saturation_water_critical = 0.13;

                double saturation_oil_irreducible_water = 0.2;
                double saturation_oil_residual_water    = 0.2;

                double saturation_oil_irreducible_gas = 0.1;
                double saturation_oil_residual_gas    = 0.1;

                double saturation_gas_connate  = 0.0;
                double saturation_gas_critical = 0.0;

                double permeability_relative_water_oil_irreducible = 0.45;

                double permeability_relative_oil_water_connate  = 1.0;
                double permeability_relative_gas_liquid_connate = 0.35;

                double exponent_permeability_relative_water     = 2.0;
                double exponent_permeability_relative_oil_water = 2.0;

                double exponent_permeability_relative_gas     = 2.0;
                double exponent_permeability_relative_oil_gas = 2.0;

                double saturation_water;
                double saturation_oil;
                double saturation_gas;

                //double[] saturations_water = Sequence.Linear(0.0, 1.0, 0.1);
                //
                double[] saturations_gas = Sequence.Linear(0.0, 1.0, 0.05);
                double[] saturations_oil = Sequence.Linear(0.0, 1.0, 0.05);
                
                Console.WriteLine("saturation_water saturation_oil saturation_gas permeability_relative_water permeability_relative_oil permeability_relative_gas");

                for(int k = 0; k < saturations_gas.Length; ++k)
                {
                    saturation_gas = saturations_gas[k];

                    for(int j = 0; j < saturations_oil.Length; ++j)
                    {
                        saturation_oil = saturations_oil[j];

                        if(saturation_oil + saturation_gas > 1.0)
                        {
                            continue;
                        }

                        saturation_water = System.Math.Max(0.0, 1.0 - saturation_oil - saturation_gas);

                        (double permeability_relative_water, double permeability_relative_oil, double permeability_relative_gas) = RelativePermeability.StoneII(saturation_water,
                            saturation_oil,
                            saturation_gas,
                            saturation_water_connate,
                            saturation_water_critical,
                            saturation_oil_irreducible_water,
                            saturation_oil_residual_water,
                            saturation_oil_irreducible_gas,
                            saturation_oil_residual_gas,
                            saturation_gas_connate,
                            saturation_gas_critical,
                            permeability_relative_water_oil_irreducible,
                            permeability_relative_oil_water_connate,
                            permeability_relative_gas_liquid_connate,
                            exponent_permeability_relative_water,
                            exponent_permeability_relative_oil_water,
                            exponent_permeability_relative_gas,
                            exponent_permeability_relative_oil_gas);

                        Console.WriteLine($"{saturation_water} {saturation_oil} {saturation_gas} {permeability_relative_water} {permeability_relative_oil} {permeability_relative_gas}");
                        
                    }
                }
            }
        }

        private static void TestMPM()
        {
            InitArguments arguments = new InitArguments(8, -1, 0, true);

            using(ScopeGuard.Get(arguments))
            {
                ExecutionSpaceKind executionSpace = ExecutionSpaceKind.Cuda;

                ReservoirProperties<double> reservoir = new ReservoirProperties<double>(executionSpace);
                reservoir.Length = 4800.0;
                reservoir.Width  = 348.0;
                // reservoir.Area                        = (reservoir.Length * reservoir.Width) / 43560;
                reservoir.Thickness             = 150.0;
                reservoir.Porosity              = 0.06;
                reservoir.Permeability          = 0.003;
                reservoir.Compressibility       = 6.11051E-05;
                reservoir.BottomholeTemperature = 275.0;
                reservoir.InitialPressure       = 7000.0;

                WellProperties<double> wellProperties = new WellProperties<double>(executionSpace);
                wellProperties.LateralLength      = 6500.0;
                wellProperties.BottomholePressure = 3500.0;

                FractureProperties<double> fracture = new FractureProperties<double>(executionSpace);
                fracture.Count        = 60;
                fracture.Width        = 0.1 / 12.0;
                fracture.Height       = 150.0;
                fracture.HalfLength   = 348.0;
                fracture.Porosity     = 0.20;
                fracture.Permeability = 184.0;
                fracture.Skin         = 0.0;

                NaturalFractureProperties<double> natural_fracture = new NaturalFractureProperties<double>(executionSpace);
                natural_fracture.Count        = 10;
                natural_fracture.Width        = 0.01 / 12.0;
                natural_fracture.Porosity     = 0.10;
                natural_fracture.Permeability = 0.8;

                //Rs = 875.99506

                Pvt<double> pvt = new Pvt<double>();
                pvt.OilSaturation            = 0.8;
                pvt.OilApiGravity            = 46.80;
                pvt.OilViscosity             = 0.11;
                pvt.OilFormationVolumeFactor = 1.56;
                pvt.OilCompressibility       = 5.993058E-05;

                pvt.WaterSaturation            = 0.0;
                pvt.WaterSpecificGravity       = 1.0;
                pvt.WaterViscosity             = 1.0;
                pvt.WaterFormationVolumeFactor = 1.0;
                pvt.WaterCompressibility       = 1.0;

                pvt.GasSaturation            = 0.2;
                pvt.GasSpecificGravity       = 0.75;
                pvt.GasViscosity             = 0.0239;
                pvt.GasFormationVolumeFactor = 1.2610E-003;
                pvt.GasCompressibilityFactor = 1.5645;
                pvt.GasCompressibility       = 2.3418E-004;

                RelativePermeabilities<double> relativePermeabilities = new RelativePermeabilities<double>();
                relativePermeabilities.MatrixOil            = 0.5;
                relativePermeabilities.MatrixWater          = 0.0;
                relativePermeabilities.MatrixGas            = 0.15;
                relativePermeabilities.FractureOil          = 0.5;
                relativePermeabilities.FractureWater        = 0.0;
                relativePermeabilities.FractureGas          = 0.15;
                relativePermeabilities.NaturalFractureOil   = 0.5;
                relativePermeabilities.NaturalFractureWater = 0.0;
                relativePermeabilities.NaturalFractureGas   = 0.15;

                MultiPorosityData<double> mpd = new MultiPorosityData<double>(executionSpace);
                mpd.ReservoirProperties       = reservoir;
                mpd.WellProperties            = wellProperties;
                mpd.FractureProperties        = fracture;
                mpd.NaturalFractureProperties = natural_fracture;
                mpd.Pvt                       = pvt;
                mpd.RelativePermeability      = relativePermeabilities;

                TriplePorosityModel<double, Cuda> tpm = new TriplePorosityModel<double, Cuda>(mpd);

                View<double, Cuda> timeView = new View<double, Cuda>("timeView", 500);

                for(ulong i0 = 0; i0 < timeView.Extent(0); ++i0)
                {
                    timeView[i0] = i0 + 1.0;
                }

                View<double, Cuda> argsView = new View<double, Cuda>("argsView", 7);

                /*km*/
                argsView[0] = 0.00019;
                /*kF*/
                argsView[1] = 184.0;
                /*kf*/
                argsView[2] = 0.8;
                /*ye*/
                argsView[3] = fracture.HalfLength;
                /*LF*/
                argsView[4] = reservoir.Length / 60.0;
                /*Lf*/
                argsView[5] = fracture.HalfLength / 10.0;
                /*sk*/
                argsView[6] = 0.0;

                View<double, Cuda> resultsView = tpm.Calculate(timeView, argsView);

                for(ulong i0 = 0; i0 < resultsView.Extent(0); ++i0)
                {
                    for(ulong i1 = 0; i1 < resultsView.Extent(1); ++i1)
                    {
                        Console.Write(resultsView[i0, i1]);
                        Console.Write(" ");
                    }

                    Console.WriteLine();
                }
            }
        }

        private static void Test()
        {
            ExecutionSpaceKind executionSpace = ExecutionSpaceKind.Cuda;

            using RrcTexasDataAdapter adapter = new RrcTexasDataAdapter();

            adapter.Context.ChangeTracker.AutoDetectChangesEnabled = false;

            //List<Well> wells = adapter.GetWellsByCounty("Karnes").Where(w => w.MonthlyProduction.Count > 0).ToList();

            EagleFordLatLong[] EagleFordLatLongs;

            using(MemoryMap mm = new MemoryMap("T:/EagleFordLatLongs.csv"))
            {
                MappedCsvReader csvReader = new MappedCsvReader(mm);

                (_, List<string[]> rows) = csvReader.ReadFile(1);

                EagleFordLatLongs = new EagleFordLatLong[rows.Count];

                Parallel.ForEach(Partitioner.Create(0, rows.Count),
                                 row =>
                                 {
                                     for(int i = row.Item1; i < row.Item2; i++)
                                     {
                                         EagleFordLatLongs[i] = new EagleFordLatLong(rows[i]);
                                     }
                                 });
            }

            //View<double, Cuda> latlongdegrees = new View<double, Cuda>("latlongdegrees", EagleFordLatLongs.Length, 2, 2);

            //for(int i = 0; i < EagleFordLatLongs.Length; ++i)
            //{
            //    latlongdegrees[i, 0, 0] = EagleFordLatLongs[i].SurfaceLatitude;
            //    latlongdegrees[i, 0, 1] = EagleFordLatLongs[i].SurfaceLongitude;
            //    latlongdegrees[i, 1, 0] = EagleFordLatLongs[i].BottomLatitude;
            //    latlongdegrees[i, 1, 1] = EagleFordLatLongs[i].BottomLongitude;
            //}

            //View<double, Cuda> neighbors = SpatialMethods<double, Cuda>.NearestNeighbor(latlongdegrees);

            InitArguments arguments = new InitArguments(8, -1, 0, true);

            using(ScopeGuard.Get(arguments))
            {
                Well well;

                foreach(EagleFordLatLong eagleFordLatLong in EagleFordLatLongs)
                {
                    well = adapter.GetWellByApi(eagleFordLatLong.Api);

                    if(well.MonthlyProduction.Count < 9)
                    {
                        continue;
                    }

                    //    Console.WriteLine($"{well.Api}");

                    ReservoirProperties<double> reservoir = new ReservoirProperties<double>(executionSpace);
                    reservoir.Length = 6500.0;
                    reservoir.Width  = 348.0;
                    // reservoir.Area                        = (reservoir.Length * reservoir.Width) / 43560;
                    reservoir.Thickness             = 50.0;
                    reservoir.Porosity              = 0.06;
                    reservoir.Permeability          = 0.002;
                    reservoir.Compressibility       = 6.11051E-05;
                    reservoir.BottomholeTemperature = 275.0;
                    reservoir.InitialPressure       = 7000.0;

                    WellProperties<double> wellProperties = new WellProperties<double>(executionSpace);
                    wellProperties.LateralLength      = 6500.0;
                    wellProperties.BottomholePressure = 3500.0;

                    FractureProperties<double> fracture = new FractureProperties<double>(executionSpace);
                    fracture.Count        = 60;
                    fracture.Width        = 0.1;
                    fracture.Height       = 50.0;
                    fracture.HalfLength   = 348.0;
                    fracture.Porosity     = 0.20;
                    fracture.Permeability = 184.0;
                    fracture.Skin         = 0.0;

                    NaturalFractureProperties<double> natural_fracture = new NaturalFractureProperties<double>(executionSpace);
                    natural_fracture.Count        = 60;
                    natural_fracture.Width        = 0.01;
                    natural_fracture.Porosity     = 0.10;
                    natural_fracture.Permeability = 1.0;

                    //Rs = 875.99506

                    Pvt<double> pvt = new Pvt<double>();
                    pvt.OilSaturation            = 0.5;
                    pvt.OilApiGravity            = 50.0;
                    pvt.OilViscosity             = 0.5;
                    pvt.OilFormationVolumeFactor = 1.5;
                    pvt.OilCompressibility       = 5.993058E-05;

                    pvt.WaterSaturation            = 0.0;
                    pvt.WaterSpecificGravity       = 1.0;
                    pvt.WaterViscosity             = 1.0;
                    pvt.WaterFormationVolumeFactor = 1.0;
                    pvt.WaterCompressibility       = 1.0;

                    pvt.GasSaturation            = 0.5;
                    pvt.GasSpecificGravity       = 0.75;
                    pvt.GasViscosity             = 0.043803267;
                    pvt.GasFormationVolumeFactor = 0.004648912;
                    pvt.GasCompressibility       = 1.5644999;

                    MultiPorosityData<double> mpd = new MultiPorosityData<double>(executionSpace);
                    mpd.ReservoirProperties       = reservoir;
                    mpd.WellProperties            = wellProperties;
                    mpd.FractureProperties        = fracture;
                    mpd.NaturalFractureProperties = natural_fracture;
                    mpd.Pvt                       = pvt;

                    BoundConstraints<double>[] arg_limits = new BoundConstraints<double>[7];

                    // LF      = xe/nF;
                    // Lf      = ye/nf;

                    /*km*/
                    arg_limits[0] = new BoundConstraints<double>(0.0001, 0.01);

                    /*kF*/
                    arg_limits[1] = new BoundConstraints<double>(100.0, 10000.0);

                    /*kf*/
                    arg_limits[2] = new BoundConstraints<double>(0.01, 100.0);

                    /*ye*/
                    arg_limits[3] = new BoundConstraints<double>(1.0, 500.0);

                    /*LF*/
                    arg_limits[4] = new BoundConstraints<double>(50.0, 250.0);

                    /*Lf*/
                    arg_limits[5] = new BoundConstraints<double>(10.0, 150.0);

                    /*sk*/
                    arg_limits[6] = new BoundConstraints<double>(0.0, 0.0);

                    View<double, Cuda> actual_data = new View<double, Cuda>("actual_data", well.MonthlyProduction.Count);

                    View<double, Cuda> actual_time = new View<double, Cuda>("actual_time", well.MonthlyProduction.Count);

                    View<double, Cuda> weights = new View<double, Cuda>("weights", well.MonthlyProduction.Count);

                    //    for(ulong i0 = 0; i0 < actual_data.Extent(0); ++i0)
                    //    {
                    //        actual_data[i0] = HunterDailyData.actualAvgDailyBoe[i0];
                    //        actual_time[i0] = HunterDailyData.actualMonthlyTime[i0];

                    //        weights[i0] = 1.0;
                    //    }

                    //    //ProductionData<double> productionData = new ProductionData<double>(well.MonthlyProduction.Count);

                    //    //for(int i = 0; i < well.MonthlyProduction.Count; ++i)
                    //    //{
                    //    //    productionData[i].Time  = well.MonthlyProduction[i].Date;
                    //    //    productionData[i].Qo    = HunterDailyData.qoData[i];
                    //    //    productionData[i].Qw    = 0.0;
                    //    //    productionData[i].Qg    = HunterDailyData.qgData[i];
                    //    //    productionData[i].QgBoe = HunterDailyData.qgData[i] / 5.8;
                    //    //    productionData[i].Qt    = productionData[i].Qo + productionData[i].QgBoe;
                    //    //}

                    TriplePorosityModel<double, Cuda> tpm = new TriplePorosityModel<double, Cuda>(mpd);

                    uint estimatedSwarmSize = ParticleSwarmOptimizationOptions.EstimateSwarmSize(7);

                    ParticleSwarmOptimizationOptions options = new ParticleSwarmOptimizationOptions(20, estimatedSwarmSize, 150, 0.0, 0.1, 0.9, false);

                    MultiPorosityResult<double, Cuda> results = tpm.HistoryMatch(options, actual_data, actual_time, weights, arg_limits);
                }
            }
        }

        private static void TestCumulativeSum()
        {
            double[] actualMonthlyBOE =
            {
                33466.0, 35563.0, 23663.0, 21862.0, 15968.0, 20670.0, 16013.0, 13683.0, 9951.0, 8713.0, 9762.0, 8241.0, 6887.0, 6293.0, 8109.0, 6763.0, 6631.0, 6953.0, 6189.0, 1618.0,
                2668.0, 2892.0, 3933.0, 3195.0, 4613.0, 4091.0, 4490.0, 2618.0, 2890.0, 2071.0, 1878.0, 3498.0, 4374.0, 3383.0, 1343.0, 633.0, 1142.0, 1009.0, 1345.0, 1158.0, 1187.0, 908.0,
                315.0, 1985.0, 1606.0, 1744.0, 1353.0, 1346.0, 1729.0, 641.0, 0.0, 1712.0, 1897.0, 1419.0, 235.0, 1379.0, 1500.0, 1636.0, 1237.0, 1209.0, 1252.0, 1200.0, 1107.0, 1098.0,
                998.0, 1076.0, 653.0, 100.0, 433.0, 876.0, 1439.0, 647.0, 2.0, 120.0, 1715.0, 1045.0
            };

            Stopwatch sw = new Stopwatch();

            sw.Start();

            double[] cumActualMonthlyBOE = OilGas.Data.Utilities.CumulativeSum(actualMonthlyBOE);

            sw.Stop();

            Console.WriteLine($"CumulativeSum {sw.ElapsedTicks}");

            sw.Reset();

            for(int i = 0; i < cumActualMonthlyBOE.Length; i++)
            {
                Console.WriteLine(cumActualMonthlyBOE[i]);
            }

            Console.WriteLine("#######################");

            Console.WriteLine($"Scan {sw.ElapsedTicks}");

            sw.Start();

            cumActualMonthlyBOE = OilGas.Data.Utilities.CumulativeSum(actualMonthlyBOE, 0, true);

            sw.Stop();

            Console.WriteLine($"Scan {sw.ElapsedTicks}");

            for(int i = 0; i < cumActualMonthlyBOE.Length; i++)
            {
                Console.WriteLine(cumActualMonthlyBOE[i]);
            }
        }

        private static void TestHistoryMatch()
        {
            ////ParallelProcessor.Initialize(new InitArguments(8,
            ////                                               -1,
            ////                                               0,
            ////                                               true));

            //InitArguments arguments = new InitArguments(8, -1, 0, true);

            //double[] values = new double[7];

            //using(ScopeGuard.Get(arguments))
            //{
            //    ProductionData<double> productionData = new ProductionData<double>(131);

            //    for(int i = 0; i < 131; ++i)
            //    {
            //        productionData[i].Time  = HunterDailyData.timeData[i];
            //        productionData[i].Qo    = HunterDailyData.qoData[i];
            //        productionData[i].Qw    = HunterDailyData.qwData[i];
            //        productionData[i].Qg    = HunterDailyData.qgData[i];
            //        productionData[i].QgBoe = HunterDailyData.qgData[i] / 5.8;
            //        productionData[i].Qt    = productionData[i].Qo + productionData[i].Qw + productionData[i].QgBoe;
            //    }

            //    ReservoirProperties<double> reservoir = new ReservoirProperties<double>();
            //    reservoir.Length = 6500.0;
            //    reservoir.Width  = 348.0;
            //    // reservoir.Area                        = (reservoir.Length * reservoir.Width) / 43560;
            //    reservoir.Thickness       = 50.0;
            //    reservoir.Porosity        = 0.06;
            //    reservoir.Permeability    = 0.002;
            //    reservoir.Temperature     = 275.0;
            //    reservoir.InitialPressure = 7000.0;

            //    WellProperties<double> well = new WellProperties<double>();
            //    well.LateralLength      = 6500.0;
            //    well.BottomholePressure = 3500.0;

            //    FractureProperties<double> fracture = new FractureProperties<double>();
            //    fracture.Count        = 60;
            //    fracture.Width        = 0.1;
            //    fracture.Height       = 50.0;
            //    fracture.HalfLength   = 348.0;
            //    fracture.Porosity     = 0.20;
            //    fracture.Permeability = 184.0;
            //    fracture.Skin         = 0.0;

            //    NaturalFractureProperties<double> natural_fracture = new NaturalFractureProperties<double>();
            //    natural_fracture.Count        = 60;
            //    natural_fracture.Width        = 0.01;
            //    natural_fracture.Porosity     = 0.10;
            //    natural_fracture.Permeability = 1.0;

            //    Pvt<double> pvt = new Pvt<double>();
            //    pvt.OilViscosity             = 0.5;
            //    pvt.OilFormationVolumeFactor = 1.5;
            //    pvt.TotalCompressibility     = 0.00002;

            //    MultiPorosityData<double> mpd = new MultiPorosityData<double>();
            //    mpd.ReservoirProperties       = reservoir;
            //    mpd.WellProperties            = well;
            //    mpd.FractureProperties        = fracture;
            //    mpd.NaturalFractureProperties = natural_fracture;
            //    mpd.Pvt                       = pvt;

            //    BoundConstraints<double>[] arg_limits = new BoundConstraints<double>[7];

            //    // LF      = xe/nF;
            //    // Lf      = ye/nf;

            //    // Hippo Hunter 1
            //    // xe = 6500
            //    // Matrix Perm (md)         1.900
            //    // Hyd Frac Perm (md)       184
            //    // # of Hyd Frac            60
            //    // Frac Half Length (ft)    348
            //    // Nat Frac Perm (md)       0.8
            //    // Total # of Nat Frac      60*10
            //    //
            //    // Hippo Hunter 2
            //    // Matrix Perm (md)         2.260
            //    // Hyd Frac Perm (md)       86
            //    // # of Hyd Frac            60
            //    // Frac Half Length (ft)	533
            //    // Nat Frac Perm (md)       0.5
            //    // Total # of Nat Frac      60*18

            //    /*km*/
            //    arg_limits[0] = new BoundConstraints<double>(0.0001, 0.01);

            //    /*kF*/
            //    arg_limits[1] = new BoundConstraints<double>(100.0, 10000.0);

            //    /*kf*/
            //    arg_limits[2] = new BoundConstraints<double>(0.01, 100.0);

            //    /*ye*/
            //    arg_limits[3] = new BoundConstraints<double>(100.0, 1000.0);

            //    /*LF*/
            //    arg_limits[4] = new BoundConstraints<double>(50.0, 250.0);

            //    /*Lf*/
            //    arg_limits[5] = new BoundConstraints<double>(10.0, 150.0);

            //    /*sk*/
            //    arg_limits[6] = new BoundConstraints<double>(0.0, 0.0);

            //    ///*km*/ arg_limits_mirror(0) = System.ValueLimits<double>(0.001, 0.002);
            //    ///*kF*/ arg_limits_mirror(1) = System.ValueLimits<double>(100.0, 200.0);
            //    ///*kf*/ arg_limits_mirror(2) = System.ValueLimits<double>(0.001, 10.0);
            //    ///*ye*/ arg_limits_mirror(3) = System.ValueLimits<double>(100.0, 500.0);
            //    ///*LF*/ arg_limits_mirror(4) = System.ValueLimits<double>(50.0, 150.0);
            //    ///*Lf*/ arg_limits_mirror(5) = System.ValueLimits<double>(10.0, 100.0);
            //    ///*sk*/ arg_limits_mirror(6) = System.ValueLimits<double>(-2.0, 2.0);

            //    // Kokkos.deep_copy(arg_limits, arg_limits_mirror);

            //    View<double, Cuda> actual_data = new View<double, Cuda>("actual_data", HunterDailyData.qoData.Length, 3);

            //    View<double, Cuda> actual_time = new View<double, Cuda>("actual_time", HunterDailyData.actualAvgDailyBoe.Length);

            //    View<double, Cuda> weights = new View<double, Cuda>("weights", HunterDailyData.actualAvgDailyBoe.Length);

            //    for(ulong i0 = 0; i0 < actual_data.Extent(0); ++i0)
            //    {
            //        actual_data[i0, 0] = HunterDailyData.qoData[i0];
            //        actual_data[i0, 1] = HunterDailyData.qwData[i0];
            //        actual_data[i0, 2] = HunterDailyData.qgData[i0];

            //        actual_time[i0]    = HunterDailyData.timeData[i0];

            //        //if(i0 < 8 || i0 >= 16 && i0 <= 17 || i0 > 66 && i0 < 90 || i0 > 125)
            //        //{
            //        //    weights[i0] = 0.0001;
            //        //}
            //        //else if(i0 >= 119 && i0 <= 125)
            //        //{
            //        //    weights[i0] = 1.2;
            //        //}
            //        //else
            //        //{
            //        weights[i0] = 1.0;
            //        //}
            //    }

            //    TriplePorosityModel<double, Cuda> tpm = new TriplePorosityModel<double, Cuda>(mpd);

            //    //NumericalMethods::Algorithms::ParticleSwarmOptimizationOptions<double> options;

            //    //Vector<double> best_args = pso.Execute(arg_limits, options);

            //    uint estimatedSwarmSize = ParticleSwarmOptimizationOptions.EstimateSwarmSize(7);

            //    ParticleSwarmOptimizationOptions
            //        options = new ParticleSwarmOptimizationOptions(100, estimatedSwarmSize, 250, 0.0, 0.1, 0.9, true); //ParticleSwarmOptimizationOptions.Default;

            //    MultiPorosityResult<double, Cuda> results = tpm.HistoryMatch(options, actual_data, actual_time, weights, arg_limits);

            //    DataCache cachedData = results.CachedData;

            //    cachedData.ExportToCsv("PSO.csv");

            //    {
            //        //Dictionary<string, string> name_map = new Dictionary<string, string>
            //        //{
            //        //    {
            //        //        "Iteration", "Iteration"
            //        //    },
            //        //    {
            //        //        "SwarmIndex", "SwarmIndex"
            //        //    },
            //        //    {
            //        //        "Particle", "ParticleIndex"
            //        //    },
            //        //    {
            //        //        "Particle0Position", "km"
            //        //    },
            //        //    {
            //        //        "Particle1Position", "kF"
            //        //    },
            //        //    {
            //        //        "Particle2Position", "kf"
            //        //    },
            //        //    {
            //        //        "Particle3Position", "ye"
            //        //    },
            //        //    {
            //        //        "Particle4Position", "LF"
            //        //    },
            //        //    {
            //        //        "Particle5Position", "Lf"
            //        //    },
            //        //    {
            //        //        "Particle6Position", "sk"
            //        //    },
            //        //    {
            //        //        "Particle0Velocity", "kmVelocity"
            //        //    },
            //        //    {
            //        //        "Particle1Velocity", "kFVelocity"
            //        //    },
            //        //    {
            //        //        "Particle2Velocity", "kfVelocity"
            //        //    },
            //        //    {
            //        //        "Particle3Velocity", "yeVelocity"
            //        //    },
            //        //    {
            //        //        "Particle4Velocity", "LFVelocity"
            //        //    },
            //        //    {
            //        //        "Particle5Velocity", "LfVelocity"
            //        //    },
            //        //    {
            //        //        "Particle6Velocity", "skVelocity"
            //        //    }
            //        //};

            //        //List<TriplePorosityOptimizationResults> dataset = new List<TriplePorosityOptimizationResults>();

            //        //for(ulong i = 0; i < cachedData.RowCount; ++i)
            //        //{
            //        //    List<double> entry = new List<double>((int)cachedData.ColumnCount);

            //        //    for(ulong j = 0; j < cachedData.ColumnCount; ++j)
            //        //    {
            //        //        entry.Add(cachedData[i,
            //        //                             j]);
            //        //    }

            //        //    dataset.Add(new TriplePorosityOptimizationResults(entry.ToArray()));
            //        //}

            //        //List<string> columnNames = new List<string>();

            //        //for(ulong i = 3; i < cachedData.ColumnCount - 2;)
            //        //{
            //        //    columnNames.Add(name_map[cachedData.GetHeader((int)i)]);

            //        //    i += 2;
            //        //}

            //        ////{
            //        ////    "$schema": "https://vega.github.io/schema/vega-lite/v4.json",
            //        ////    "description": "Drag the sliders to highlight points.",
            //        ////    "data": {"url": "data/cars.json"},
            //        ////    "transform": [{"calculate": "year(datum.Year)", "as": "Year"}],
            //        ////    "layer": [{
            //        ////        "selection": {
            //        ////            "CylYr": {
            //        ////                "type": "single", "fields": ["Cylinders", "Year"],
            //        ////                "init": {"Cylinders": 4, "Year": 1977},
            //        ////                "bind": {
            //        ////                    "Cylinders": {"input": "range", "min": 3, "max": 8, "step": 1},
            //        ////                    "Year": {"input": "range", "min": 1969, "max": 1981, "step": 1}
            //        ////                }
            //        ////            }
            //        ////        },
            //        ////        "mark": "circle",
            //        ////        "encoding": {
            //        ////            "x": {"field": "Horsepower", "type": "quantitative"},
            //        ////            "y": {"field": "Miles_per_Gallon", "type": "quantitative"},
            //        ////            "color": {
            //        ////                "condition": {"selection": "CylYr", "field": "Origin", "type": "nominal"},
            //        ////                "value": "grey"
            //        ////            }
            //        ////        }
            //        ////    }, {
            //        ////        "transform": [{"filter": {"selection": "CylYr"}}],
            //        ////        "mark": "circle",
            //        ////        "encoding": {
            //        ////            "x": {"field": "Horsepower", "type": "quantitative"},
            //        ////            "y": {"field": "Miles_per_Gallon", "type": "quantitative"},
            //        ////            "color": {"field": "Origin", "type": "nominal"},
            //        ////            "size": {"value": 100}
            //        ////        }
            //        ////    }]
            //        ////}

            //        //Specification specification = new Specification
            //        //{
            //        //    Transform = new List<Transform>
            //        //    {
            //        //        new Transform
            //        //        {
            //        //            Filter = new Predicate
            //        //            {
            //        //                Selection = "Iteration"
            //        //            }
            //        //        }
            //        //    },
            //        //    Repeat = new RepeatMapping
            //        //    {
            //        //        Row = columnNames, Column = columnNames
            //        //    },
            //        //    Spec = new SpecClass
            //        //    {
            //        //        DataSource = new DataSource
            //        //        {
            //        //            Name = nameof(dataset)
            //        //        },
            //        //        Mark = BoxPlot.Circle,
            //        //        Encoding = new Encoding
            //        //        {
            //        //            X = new XClass
            //        //            {
            //        //                Type = StandardType.Quantitative,
            //        //                Field = new RepeatRef
            //        //                {
            //        //                    Repeat = RepeatEnum.Column
            //        //                }
            //        //            },
            //        //            Y = new YClass
            //        //            {
            //        //                Type = StandardType.Quantitative,
            //        //                Field = new RepeatRef
            //        //                {
            //        //                    Repeat = RepeatEnum.Row
            //        //                }
            //        //            },
            //        //            Color = new DefWithConditionMarkPropFieldDefGradientStringNull
            //        //            {
            //        //                Type = StandardType.Nominal, Field = "SwarmIndex"
            //        //            }
            //        //        },
            //        //        Selection = new Dictionary<string, SelectionDef>
            //        //        {
            //        //            {
            //        //                "IterationSelection", new SelectionDef
            //        //                {
            //        //                    Type = SelectionDefType.Single,
            //        //                    Fields = new List<string>
            //        //                    {
            //        //                        "Iteration"
            //        //                    },
            //        //                    Init = new Dictionary<string, InitValue>
            //        //                    {
            //        //                        {
            //        //                            "Iteration", 0
            //        //                        }
            //        //                    },
            //        //                    Bind = new Dictionary<string, AnyStream>
            //        //                    {
            //        //                        {
            //        //                            "Iteration", new AnyBinding
            //        //                            {
            //        //                                Input = "range",
            //        //                                Min   = 0.0,
            //        //                                Max   = 99.0
            //        //                            }
            //        //                        }
            //        //                    }
            //        //                }
            //        //            }
            //        //        }
            //        //    }
            //        //};

            //        ////Chart chart = new Chart($"TriplePorosityModel",
            //        ////                        specification,
            //        ////                        1000,
            //        ////                        750);

            //        ////chart.ShowInBrowser();
            //    }

            //    //Console.WriteLine("RMS Error");
            //    //Console.WriteLine(results.Error);

            //    View<double, Cuda> best_args = results.Args;

            //    Console.WriteLine("best_args");

            //    for(ulong i = 0; i < best_args.Size(); ++i)
            //    {
            //        values[i] = best_args[i];

            //        Console.WriteLine(values[i]);
            //    }

            //    //Console.WriteLine();

            //    ////View<double, Cuda> timeView = new View<double, Cuda>("time",
            //    ////                                                     4);

            //    ////for(ulong i0 = 0; i0 < timeView.Extent(0); ++i0)
            //    ////{
            //    ////    timeView[i0] = 15 + 30 * i0;
            //    ////}

            //    //View<double, Cuda> best_args = new View<double, Cuda>("args",
            //    //                                                      7);

            //    //double[] values =
            //    //{
            //    //    0.006064035, 451.7930851, 4.277795829, 177.2940392, 77.86379899, 73.58321739 ,0.0
            //    //};

            //    //best_args[0] = values[0];
            //    //best_args[1] = values[1];
            //    //best_args[2] = values[2];
            //    //best_args[3] = values[3];
            //    //best_args[4] = values[4];
            //    //best_args[5] = values[5];
            //    //best_args[6] = values[6];

            //    View<double, Cuda> simulated_data = tpm.Calculate(actual_time, best_args);

            //    Console.WriteLine("simulated_data");

            //    for(ulong i0 = 0; i0 < simulated_data.Size(); ++i0)
            //    {
            //        Console.Write(simulated_data[i0, 0]);
            //        Console.Write(" ");
            //        Console.Write(simulated_data[i0, 1]);
            //        Console.Write(" ");
            //        Console.Write(simulated_data[i0, 2]);
            //    }

            //    {
            //        //List<InlineDatasetElement> dataset = new List<InlineDatasetElement>();

            //        //for(ulong i = 0; i < actual_data.Size(); ++i)
            //        //{
            //        //    dataset.Add(new Dictionary<string, object>(4)
            //        //    {
            //        //        {
            //        //            "API", "##-###-#####"
            //        //        },
            //        //        {
            //        //            "Day", actual_time[i]
            //        //        },
            //        //        {
            //        //            "Type", "Actual"
            //        //        },
            //        //        {
            //        //            "Rate", actual_data[i]
            //        //        }
            //        //    });
            //        //}

            //        //for(ulong i = 0; i < simulated_data.Size(); ++i)
            //        //{
            //        //    dataset.Add(new Dictionary<string, object>(4)
            //        //    {
            //        //        {
            //        //            "API", "##-###-#####"
            //        //        },
            //        //        {
            //        //            "Day", actual_time[i]
            //        //        },
            //        //        {
            //        //            "Type", "Simulated"
            //        //        },
            //        //        {
            //        //            "Rate", simulated_data[i]
            //        //        }
            //        //    });
            //        //}

            //        //Specification specification = new Specification
            //        //{
            //        //    Data = new DataSource
            //        //    {
            //        //        Values = dataset
            //        //    },
            //        //    Layer = new List<LayerSpec>
            //        //    {
            //        //        new LayerSpec
            //        //        {
            //        //            Encoding = new LayerEncoding
            //        //            {
            //        //                X = new XClass
            //        //                {
            //        //                    Type = StandardType.Quantitative, Field = "Day"
            //        //                },
            //        //                Y = new YClass
            //        //                {
            //        //                    Type = StandardType.Quantitative, Field = "Rate"
            //        //                },
            //        //                Color = new DefWithConditionMarkPropFieldDefGradientStringNull
            //        //                {
            //        //                    Type = StandardType.Nominal, Field = "Type"
            //        //                }
            //        //            },
            //        //            Layer = new List<LayerSpec>
            //        //            {
            //        //                new LayerSpec
            //        //                {
            //        //                    Mark = BoxPlot.Line
            //        //                },
            //        //                new LayerSpec
            //        //                {
            //        //                    Mark = BoxPlot.Circle
            //        //                }
            //        //            }
            //        //        }
            //        //    }
            //        //};

            //        //Chart chart = new Chart("TriplePorosityModel",
            //        //                        specification,
            //        //                        1000,
            //        //                        750);

            //        //chart.ShowInBrowser();
            //    }
            //}

            ////ParallelProcessor.Shutdown();
        }

        private static void TestPrediction()
        {
            ////ParallelProcessor.Initialize(new InitArguments(8,
            ////                                               -1,
            ////                                               0,
            ////                                               true));

            //InitArguments arguments = new InitArguments(8, -1, 0, true);

            //double[] values = new double[7];

            //using(ScopeGuard.Get(arguments))
            //{
            //    ProductionData<double> productionData = new ProductionData<double>(131);

            //    for(int i = 0; i < 131; ++i)
            //    {
            //        productionData[i].Time  = HunterDailyData.timeData[i];
            //        productionData[i].Qo    = HunterDailyData.qoData[i];
            //        productionData[i].Qw    = HunterDailyData.qwData[i];
            //        productionData[i].Qg    = HunterDailyData.qgData[i];
            //        productionData[i].QgBoe = HunterDailyData.qgData[i] / 5.8;
            //        productionData[i].Qt    = productionData[i].Qo + productionData[i].Qw + productionData[i].QgBoe;
            //    }

            //    ReservoirProperties<double> reservoir = new ReservoirProperties<double>();
            //    reservoir.Length = 6500.0;
            //    reservoir.Width  = 348.0;
            //    // reservoir.Area                        = (reservoir.Length * reservoir.Width) / 43560;
            //    reservoir.Thickness       = 125.0;
            //    reservoir.Porosity        = 0.06;
            //    reservoir.Permeability    = 0.002;
            //    reservoir.Temperature     = 275.0;
            //    reservoir.InitialPressure = 7000.0;

            //    WellProperties<double> well = new WellProperties<double>();
            //    well.LateralLength      = 6500.0;
            //    well.BottomholePressure = 3500.0;

            //    FractureProperties<double> fracture = new FractureProperties<double>();
            //    fracture.Count        = 60;
            //    fracture.Width        = 0.1;
            //    fracture.Height       = 50.0;
            //    fracture.HalfLength   = 348.0;
            //    fracture.Porosity     = 0.20;
            //    fracture.Permeability = 184.0;
            //    fracture.Skin         = 0.0;

            //    NaturalFractureProperties<double> natural_fracture = new NaturalFractureProperties<double>();
            //    natural_fracture.Count        = 60;
            //    natural_fracture.Width        = 0.01;
            //    natural_fracture.Porosity     = 0.10;
            //    natural_fracture.Permeability = 1.0;

            //    Pvt<double> pvt = new Pvt<double>();
            //    pvt.OilViscosity             = 0.5;
            //    pvt.OilFormationVolumeFactor = 1.5;
            //    pvt.TotalCompressibility     = 0.00002;

            //    MultiPorosityData<double> mpd = new MultiPorosityData<double>();
            //    mpd.ProductionData            = productionData;
            //    mpd.ReservoirProperties       = reservoir;
            //    mpd.WellProperties            = well;
            //    mpd.FractureProperties        = fracture;
            //    mpd.NaturalFractureProperties = natural_fracture;
            //    mpd.Pvt                       = pvt;

            //    BoundConstraints<double>[] arg_limits = new BoundConstraints<double>[7];

            //    // LF      = xe/nF;
            //    // Lf      = ye/nf;

            //    // Hippo Hunter 1
            //    // xe = 6500
            //    // Matrix Perm (md)         1.900
            //    // Hyd Frac Perm (md)       184
            //    // # of Hyd Frac            60
            //    // Frac Half Length (ft)    348
            //    // Nat Frac Perm (md)       0.8
            //    // Total # of Nat Frac      60*10
            //    //
            //    // Hippo Hunter 2
            //    // Matrix Perm (md)         2.260
            //    // Hyd Frac Perm (md)       86
            //    // # of Hyd Frac            60
            //    // Frac Half Length (ft)	533
            //    // Nat Frac Perm (md)       0.5
            //    // Total # of Nat Frac      60*18

            //    /*km*/
            //    arg_limits[0] = new BoundConstraints<double>(0.0001, 0.01);

            //    /*kF*/
            //    arg_limits[1] = new BoundConstraints<double>(100.0, 10000.0);

            //    /*kf*/
            //    arg_limits[2] = new BoundConstraints<double>(0.01, 1000.0);

            //    /*ye*/
            //    arg_limits[3] = new BoundConstraints<double>(100.0, 1000.0);

            //    /*LF*/
            //    arg_limits[4] = new BoundConstraints<double>(50.0, 250.0);

            //    /*Lf*/
            //    arg_limits[5] = new BoundConstraints<double>(10.0, 150.0);

            //    /*sk*/
            //    arg_limits[6] = new BoundConstraints<double>(0.0, 0.0);

            //    ///*km*/ arg_limits_mirror(0) = System.ValueLimits<double>(0.001, 0.002);
            //    ///*kF*/ arg_limits_mirror(1) = System.ValueLimits<double>(100.0, 200.0);
            //    ///*kf*/ arg_limits_mirror(2) = System.ValueLimits<double>(0.001, 10.0);
            //    ///*ye*/ arg_limits_mirror(3) = System.ValueLimits<double>(100.0, 500.0);
            //    ///*LF*/ arg_limits_mirror(4) = System.ValueLimits<double>(50.0, 150.0);
            //    ///*Lf*/ arg_limits_mirror(5) = System.ValueLimits<double>(10.0, 100.0);
            //    ///*sk*/ arg_limits_mirror(6) = System.ValueLimits<double>(-2.0, 2.0);

            //    // Kokkos.deep_copy(arg_limits, arg_limits_mirror);

            //    int months = 36;

            //    View<double, Cuda> actual_data = new View<double, Cuda>("actual_data", months);

            //    View<double, Cuda> actual_time = new View<double, Cuda>("actual_time", months);

            //    View<double, Cuda> weights = new View<double, Cuda>("weights", months);

            //    for(ulong i0 = 0; i0 < actual_data.Extent(0); ++i0)
            //    {
            //        actual_data[i0] = HunterDailyData.actualAvgDailyBoe[i0]; //productionData[i0].Qt;
            //        actual_time[i0] = HunterDailyData.actualMonthlyTime[i0]; //timeData[i0];

            //        //if(i0 < 8 || i0 >= 16 && i0 <= 17 || i0 > 66 && i0 < 90 || i0 > 125)
            //        //{
            //        //    weights[i0] = 0.0001;
            //        //}
            //        //else if(i0 >= 119 && i0 <= 125)
            //        //{
            //        //    weights[i0] = 1.2;
            //        //}
            //        //else
            //        //{
            //        weights[i0] = 1.0;
            //        //}
            //    }

            //    TriplePorosityModel<double, Cuda> tpm = new TriplePorosityModel<double, Cuda>(mpd);

            //    //NumericalMethods::Algorithms::ParticleSwarmOptimizationOptions<double> options;

            //    //Vector<double> best_args = pso.Execute(arg_limits, options);
            //    ParticleSwarmOptimizationOptions options = new ParticleSwarmOptimizationOptions(100, 10, 250, 0.0, 0.1, 0.9, false); //ParticleSwarmOptimizationOptions.Default;

            //    MultiPorosityResult<double, Cuda> results = tpm.HistoryMatch(options, actual_data, actual_time, weights, arg_limits);

            //    //DataCache cachedData = results.CachedData;

            //    {
            //        //Dictionary<string, string> name_map = new Dictionary<string, string>
            //        //{
            //        //    {
            //        //        "Iteration", "Iteration"
            //        //    },
            //        //    {
            //        //        "SwarmIndex", "SwarmIndex"
            //        //    },
            //        //    {
            //        //        "Particle", "ParticleIndex"
            //        //    },
            //        //    {
            //        //        "Particle0Position", "km"
            //        //    },
            //        //    {
            //        //        "Particle1Position", "kF"
            //        //    },
            //        //    {
            //        //        "Particle2Position", "kf"
            //        //    },
            //        //    {
            //        //        "Particle3Position", "ye"
            //        //    },
            //        //    {
            //        //        "Particle4Position", "LF"
            //        //    },
            //        //    {
            //        //        "Particle5Position", "Lf"
            //        //    },
            //        //    {
            //        //        "Particle6Position", "sk"
            //        //    },
            //        //    {
            //        //        "Particle0Velocity", "kmVelocity"
            //        //    },
            //        //    {
            //        //        "Particle1Velocity", "kFVelocity"
            //        //    },
            //        //    {
            //        //        "Particle2Velocity", "kfVelocity"
            //        //    },
            //        //    {
            //        //        "Particle3Velocity", "yeVelocity"
            //        //    },
            //        //    {
            //        //        "Particle4Velocity", "LFVelocity"
            //        //    },
            //        //    {
            //        //        "Particle5Velocity", "LfVelocity"
            //        //    },
            //        //    {
            //        //        "Particle6Velocity", "skVelocity"
            //        //    }
            //        //};

            //        //List<TriplePorosityOptimizationResults> dataset = new List<TriplePorosityOptimizationResults>();

            //        //for(ulong i = 0; i < cachedData.RowCount; ++i)
            //        //{
            //        //    List<double> entry = new List<double>((int)cachedData.ColumnCount);

            //        //    for(ulong j = 0; j < cachedData.ColumnCount; ++j)
            //        //    {
            //        //        entry.Add(cachedData[i,
            //        //                             j]);
            //        //    }

            //        //    dataset.Add(new TriplePorosityOptimizationResults(entry.ToArray()));
            //        //}

            //        //List<string> columnNames = new List<string>();

            //        //for(ulong i = 3; i < cachedData.ColumnCount - 2;)
            //        //{
            //        //    columnNames.Add(name_map[cachedData.GetHeader((int)i)]);

            //        //    i += 2;
            //        //}

            //        ////{
            //        ////    "$schema": "https://vega.github.io/schema/vega-lite/v4.json",
            //        ////    "description": "Drag the sliders to highlight points.",
            //        ////    "data": {"url": "data/cars.json"},
            //        ////    "transform": [{"calculate": "year(datum.Year)", "as": "Year"}],
            //        ////    "layer": [{
            //        ////        "selection": {
            //        ////            "CylYr": {
            //        ////                "type": "single", "fields": ["Cylinders", "Year"],
            //        ////                "init": {"Cylinders": 4, "Year": 1977},
            //        ////                "bind": {
            //        ////                    "Cylinders": {"input": "range", "min": 3, "max": 8, "step": 1},
            //        ////                    "Year": {"input": "range", "min": 1969, "max": 1981, "step": 1}
            //        ////                }
            //        ////            }
            //        ////        },
            //        ////        "mark": "circle",
            //        ////        "encoding": {
            //        ////            "x": {"field": "Horsepower", "type": "quantitative"},
            //        ////            "y": {"field": "Miles_per_Gallon", "type": "quantitative"},
            //        ////            "color": {
            //        ////                "condition": {"selection": "CylYr", "field": "Origin", "type": "nominal"},
            //        ////                "value": "grey"
            //        ////            }
            //        ////        }
            //        ////    }, {
            //        ////        "transform": [{"filter": {"selection": "CylYr"}}],
            //        ////        "mark": "circle",
            //        ////        "encoding": {
            //        ////            "x": {"field": "Horsepower", "type": "quantitative"},
            //        ////            "y": {"field": "Miles_per_Gallon", "type": "quantitative"},
            //        ////            "color": {"field": "Origin", "type": "nominal"},
            //        ////            "size": {"value": 100}
            //        ////        }
            //        ////    }]
            //        ////}

            //        //Specification specification = new Specification
            //        //{
            //        //    Transform = new List<Transform>
            //        //    {
            //        //        new Transform
            //        //        {
            //        //            Filter = new Predicate
            //        //            {
            //        //                Selection = "Iteration"
            //        //            }
            //        //        }
            //        //    },
            //        //    Repeat = new RepeatMapping
            //        //    {
            //        //        Row = columnNames, Column = columnNames
            //        //    },
            //        //    Spec = new SpecClass
            //        //    {
            //        //        DataSource = new DataSource
            //        //        {
            //        //            Name = nameof(dataset)
            //        //        },
            //        //        Mark = BoxPlot.Circle,
            //        //        Encoding = new Encoding
            //        //        {
            //        //            X = new XClass
            //        //            {
            //        //                Type = StandardType.Quantitative,
            //        //                Field = new RepeatRef
            //        //                {
            //        //                    Repeat = RepeatEnum.Column
            //        //                }
            //        //            },
            //        //            Y = new YClass
            //        //            {
            //        //                Type = StandardType.Quantitative,
            //        //                Field = new RepeatRef
            //        //                {
            //        //                    Repeat = RepeatEnum.Row
            //        //                }
            //        //            },
            //        //            Color = new DefWithConditionMarkPropFieldDefGradientStringNull
            //        //            {
            //        //                Type = StandardType.Nominal, Field = "SwarmIndex"
            //        //            }
            //        //        },
            //        //        Selection = new Dictionary<string, SelectionDef>
            //        //        {
            //        //            {
            //        //                "IterationSelection", new SelectionDef
            //        //                {
            //        //                    Type = SelectionDefType.Single,
            //        //                    Fields = new List<string>
            //        //                    {
            //        //                        "Iteration"
            //        //                    },
            //        //                    Init = new Dictionary<string, InitValue>
            //        //                    {
            //        //                        {
            //        //                            "Iteration", 0
            //        //                        }
            //        //                    },
            //        //                    Bind = new Dictionary<string, AnyStream>
            //        //                    {
            //        //                        {
            //        //                            "Iteration", new AnyBinding
            //        //                            {
            //        //                                Input = "range",
            //        //                                Min   = 0.0,
            //        //                                Max   = 99.0
            //        //                            }
            //        //                        }
            //        //                    }
            //        //                }
            //        //            }
            //        //        }
            //        //    }
            //        //};

            //        ////Chart chart = new Chart($"TriplePorosityModel",
            //        ////                        specification,
            //        ////                        1000,
            //        ////                        750);

            //        ////chart.ShowInBrowser();
            //    }

            //    //Console.WriteLine("RMS Error");
            //    //Console.WriteLine(results.Error);

            //    View<double, Cuda> best_args = results.Args;

            //    Console.WriteLine("best_args");

            //    for(ulong i = 0; i < best_args.Size(); ++i)
            //    {
            //        values[i] = best_args[i];

            //        Console.WriteLine(values[i]);
            //    }

            //    //Console.WriteLine();

            //    ////View<double, Cuda> timeView = new View<double, Cuda>("time",
            //    ////                                                     4);

            //    ////for(ulong i0 = 0; i0 < timeView.Extent(0); ++i0)
            //    ////{
            //    ////    timeView[i0] = 15 + 30 * i0;
            //    ////}

            //    //View<double, Cuda> best_args = new View<double, Cuda>("args",
            //    //                                                      7);

            //    //double[] values =
            //    //{
            //    //    0.006064035, 451.7930851, 4.277795829, 177.2940392, 77.86379899, 73.58321739 ,0.0
            //    //};

            //    //best_args[0] = values[0];
            //    //best_args[1] = values[1];
            //    //best_args[2] = values[2];
            //    //best_args[3] = values[3];
            //    //best_args[4] = values[4];
            //    //best_args[5] = values[5];
            //    //best_args[6] = values[6];

            //    View<double, Cuda> simulated_data = tpm.Calculate(actual_time, best_args);

            //    Console.WriteLine("simulated_data");

            //    for(ulong i0 = 0; i0 < simulated_data.Size(); ++i0)
            //    {
            //        Console.WriteLine(simulated_data[i0]);
            //    }

            //    {
            //        //List<InlineDatasetElement> dataset = new List<InlineDatasetElement>();

            //        //for(ulong i = 0; i < actual_data.Size(); ++i)
            //        //{
            //        //    dataset.Add(new Dictionary<string, object>(4)
            //        //    {
            //        //        {
            //        //            "API", "##-###-#####"
            //        //        },
            //        //        {
            //        //            "Day", actual_time[i]
            //        //        },
            //        //        {
            //        //            "Type", "Actual"
            //        //        },
            //        //        {
            //        //            "Rate", actual_data[i]
            //        //        }
            //        //    });
            //        //}

            //        //for(ulong i = 0; i < simulated_data.Size(); ++i)
            //        //{
            //        //    dataset.Add(new Dictionary<string, object>(4)
            //        //    {
            //        //        {
            //        //            "API", "##-###-#####"
            //        //        },
            //        //        {
            //        //            "Day", actual_time[i]
            //        //        },
            //        //        {
            //        //            "Type", "Simulated"
            //        //        },
            //        //        {
            //        //            "Rate", simulated_data[i]
            //        //        }
            //        //    });
            //        //}

            //        //Specification specification = new Specification
            //        //{
            //        //    Data = new DataSource
            //        //    {
            //        //        Values = dataset
            //        //    },
            //        //    Layer = new List<LayerSpec>
            //        //    {
            //        //        new LayerSpec
            //        //        {
            //        //            Encoding = new LayerEncoding
            //        //            {
            //        //                X = new XClass
            //        //                {
            //        //                    Type = StandardType.Quantitative, Field = "Day"
            //        //                },
            //        //                Y = new YClass
            //        //                {
            //        //                    Type = StandardType.Quantitative, Field = "Rate"
            //        //                },
            //        //                Color = new DefWithConditionMarkPropFieldDefGradientStringNull
            //        //                {
            //        //                    Type = StandardType.Nominal, Field = "Type"
            //        //                }
            //        //            },
            //        //            Layer = new List<LayerSpec>
            //        //            {
            //        //                new LayerSpec
            //        //                {
            //        //                    Mark = BoxPlot.Line
            //        //                },
            //        //                new LayerSpec
            //        //                {
            //        //                    Mark = BoxPlot.Circle
            //        //                }
            //        //            }
            //        //        }
            //        //    }
            //        //};

            //        //Chart chart = new Chart("TriplePorosityModel",
            //        //                        specification,
            //        //                        1000,
            //        //                        750);

            //        //chart.ShowInBrowser();
            //    }
            //}

            ////ParallelProcessor.Shutdown();

            //// Prediction

            //double[] predictTimeData = HunterDailyData.actualMonthlyTime; //Sequence.Linear(1.0, 2299.0, 1.0);
            //double[] predictQt;

            //using(ScopeGuard.Get(arguments))
            //{
            //    ProductionData<double> productionData = new ProductionData<double>(131);

            //    for(int i = 0; i < 131; ++i)
            //    {
            //        productionData[i].Time  = HunterDailyData.timeData[i];
            //        productionData[i].Qo    = HunterDailyData.qoData[i];
            //        productionData[i].Qw    = 0.0;
            //        productionData[i].Qg    = HunterDailyData.qgData[i];
            //        productionData[i].QgBoe = HunterDailyData.qgData[i] / 5.8;
            //        productionData[i].Qt    = productionData[i].Qo + productionData[i].QgBoe;
            //    }

            //    ReservoirProperties<double> reservoir = new ReservoirProperties<double>();
            //    reservoir.Length = 6500.0;
            //    reservoir.Width  = 348.0;
            //    // reservoir.Area                        = (reservoir.Length * reservoir.Width) / 43560;
            //    reservoir.Thickness       = 125.0;
            //    reservoir.Porosity        = 0.06;
            //    reservoir.Permeability    = 0.002;
            //    reservoir.Temperature     = 275.0;
            //    reservoir.InitialPressure = 7000.0;

            //    WellProperties<double> well = new WellProperties<double>();
            //    well.LateralLength      = 6500.0;
            //    well.BottomholePressure = 3500.0;

            //    FractureProperties<double> fracture = new FractureProperties<double>();
            //    fracture.Count        = 60;
            //    fracture.Width        = 0.1;
            //    fracture.Height       = 50.0;
            //    fracture.HalfLength   = 348.0;
            //    fracture.Porosity     = 0.20;
            //    fracture.Permeability = 184.0;
            //    fracture.Skin         = 0.0;

            //    NaturalFractureProperties<double> natural_fracture = new NaturalFractureProperties<double>();
            //    natural_fracture.Count        = 60;
            //    natural_fracture.Width        = 0.01;
            //    natural_fracture.Porosity     = 0.10;
            //    natural_fracture.Permeability = 1.0;

            //    Pvt<double> pvt = new Pvt<double>();
            //    pvt.OilViscosity             = 0.5;
            //    pvt.OilFormationVolumeFactor = 1.5;
            //    pvt.TotalCompressibility     = 0.00002;

            //    MultiPorosityData<double> mpd = new MultiPorosityData<double>();
            //    mpd.ProductionData            = productionData;
            //    mpd.ReservoirProperties       = reservoir;
            //    mpd.WellProperties            = well;
            //    mpd.FractureProperties        = fracture;
            //    mpd.NaturalFractureProperties = natural_fracture;
            //    mpd.Pvt                       = pvt;

            //    View<double, Cuda> actualQt = new View<double, Cuda>("actualQt", HunterDailyData.actualAvgDailyBoe.Length);

            //    View<double, Cuda> actualTime = new View<double, Cuda>("actualTime", HunterDailyData.actualAvgDailyBoe.Length);

            //    View<double, Cuda> weights = new View<double, Cuda>("weights", HunterDailyData.actualAvgDailyBoe.Length);

            //    for(ulong i0 = 0; i0 < actualQt.Extent(0); ++i0)
            //    {
            //        actualQt[i0]   = HunterDailyData.actualAvgDailyBoe[i0]; //productionData[i0].Qt;
            //        actualTime[i0] = HunterDailyData.actualMonthlyTime[i0]; //timeData[i0];

            //        //if(i0 < 8 || i0 >= 16 && i0 <= 17 || i0 > 66 && i0 < 90 || i0 > 125)
            //        //{
            //        //    weights[i0] = 0.0001;
            //        //}
            //        //else if(i0 >= 119 && i0 <= 125)
            //        //{
            //        //    weights[i0] = 1.2;
            //        //}
            //        //else
            //        //{
            //        weights[i0] = 1.0;
            //        //}
            //    }

            //    TriplePorosityModel<double, Cuda> tpm = new TriplePorosityModel<double, Cuda>(mpd);

            //    View<double, Cuda> args = new View<double, Cuda>("args", 7);

            //    //km: 0.0018895176183702685
            //    //kF: 83.69857529340902
            //    //kf: 4.762316834147342
            //    //ye: 162.8781340630174
            //    //LF: 148.64640077973382
            //    //Lf: 55.506016771447456
            //    //sk: 0.0

            //    //args[0] = 0.0018895176183702685;
            //    //args[1] = 83.69857529340902;
            //    //args[2] = 4.762316834147342;
            //    //args[3] = 162.8781340630174;
            //    //args[4] = 148.64640077973382;
            //    //args[5] = 55.506016771447456;
            //    //args[6] = 0.0;

            //    for(ulong i = 0; i < args.Size(); ++i)
            //    {
            //        args[i] = values[i];
            //    }

            //    View<double, Cuda> predictedTimeData = new View<double, Cuda>("predictedTimeData", predictTimeData.Length);

            //    for(int i0 = 0; i0 < predictTimeData.Length; ++i0)
            //    {
            //        predictedTimeData[i0] = predictTimeData[i0];
            //    }

            //    View<double, Cuda> predictedQt = tpm.Calculate(predictedTimeData, args);

            //    predictQt = new double[predictedQt.Size()];

            //    for(ulong i0 = 0; i0 < predictedQt.Size(); ++i0)
            //    {
            //        predictQt[i0] = predictedQt[i0];
            //    }
            //}

            //ProductionChartCollection predict_results = new ProductionChartCollection(HunterDailyData.actualMonthlyBoe.Length + predictTimeData.Length);

            //double t;

            //for(int i0 = 0; i0 < predictTimeData.Length; ++i0)
            //{
            //    t = predictTimeData[i0];
            //    predict_results.Add("Predicted", t, predictQt[i0]);
            //}

            ////predict_results.ToMonthlyProduction(new System.DateTime(2012, 10, 15), 15);

            //for(int i0 = 0; i0 < HunterDailyData.actualMonthlyBoe.Length; ++i0)
            //{
            //    t = HunterDailyData.actualMonthlyTime[i0];
            //    predict_results.Add("Actual", t, HunterDailyData.actualMonthlyBoe[i0]);
            //}

            //predict_results.BuildCumulativeProduction();

            //for(int i0 = 0; i0 < predict_results["Predicted"].Count; ++i0)
            //{
            //    Console.WriteLine($"{predict_results["Predicted"][i0].Day} {predict_results["Predicted"][i0].Rate}");
            //}
        }

        private readonly struct EagleFordLatLong
        {
            public readonly ApiNumber Api;
            public readonly double    SurfaceLatitude;
            public readonly double    SurfaceLongitude;
            public readonly double    BottomLatitude;
            public readonly double    BottomLongitude;

            public EagleFordLatLong(string[] row)
            {
                if(row.Length != 5)
                {
                    throw new InvalidDataException();
                }

                Api              = row[0];
                SurfaceLatitude  = double.Parse(row[1]);
                SurfaceLongitude = double.Parse(row[2]);
                BottomLatitude   = double.Parse(row[3]);
                BottomLongitude  = double.Parse(row[4]);
            }
        }
    }
}