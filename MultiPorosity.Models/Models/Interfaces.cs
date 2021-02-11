using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using PlatformApi.Win32;

namespace MultiPorosity.Models
{
    [Guid("F0000000-F000-F000-F000-F00000000100")]
    public interface IReservoirProperties<T> : IUnknown
    {
        T getLength();

        void setLength(T value);

        T getWidth();

        void setWidth(T value);

        T getThickness();

        void setThickness(T value);

        T getPorosity();

        void setPorosity(T value);

        T getPermeability();

        void setPermeability(T value);

        T getTemperature();

        void setTemperature(T value);

        T getInitialPressure();

        void setInitialPressure(T value);
    }

    [Guid("F0000000-F000-F000-F000-F00000000101")]
    public interface IReservoirPropertiesD : IReservoirProperties<double>
    {
    }

    [Guid("F0000000-F000-F000-F000-F00000000102")]
    public interface IReservoirPropertiesF : IReservoirProperties<float>
    {
    }

    [Guid("F0000000-F000-F000-F000-F00000000200")]
    public interface IWellProperties<T> : IUnknown
    {
        ulong getAPI();

        void setAPI(ulong value);

        T getLatLen();

        void setLatLen(T value);

        T getBHP();

        void setBHP(T value);
    }

    [Guid("F0000000-F000-F000-F000-F00000000201")]
    public interface IWellPropertiesD : IWellProperties<double>
    {
    }

    [Guid("F0000000-F000-F000-F000-F00000000202")]
    public interface IWellPropertiesF : IWellProperties<float>
    {
    }

    [Guid("F0000000-F000-F000-F000-F00000000300")]
    public interface IFractureProperties<T> : IUnknown
    {
        int getCount();

        void setCount(int value);

        T getWidth();

        void setWidth(T value);

        T getHeight();

        void setHeight(T value);

        T getHalfLength();

        void setHalfLength(T value);

        T getPorosity();

        void setPorosity(T value);

        T getPermeability();

        void setPermeability(T value);

        T getSkin();

        void setSkin(T value);
    }

    [Guid("F0000000-F000-F000-F000-F00000000301")]
    public interface IFracturePropertiesD : IFractureProperties<double>
    {
    }

    [Guid("F0000000-F000-F000-F000-F00000000302")]
    public interface IFracturePropertiesF : IFractureProperties<float>
    {
    }

    [Guid("F0000000-F000-F000-F000-F00000000400")]
    public interface INaturalFractureProperties<T> : IUnknown
    {
        int getCount();

        void setCount(int value);

        T getWidth();

        void setWidth(T value);

        T getPorosity();

        void setPorosity(T value);

        T getPermeability();

        void setPermeability(T value);
    }

    [Guid("F0000000-F000-F000-F000-F00000000401")]
    public interface INaturalFracturePropertiesD : INaturalFractureProperties<double>
    {
    }

    [Guid("F0000000-F000-F000-F000-F00000000402")]
    public interface INaturalFracturePropertiesF : INaturalFractureProperties<float>
    {
    }

    [Guid("F0000000-F000-F000-F000-F00000000500")]
    public interface IPvt<T> : IUnknown
    {
        T getmu();

        void setmu(T value);

        T getBo();

        void setBo(T value);

        T gettotalComprss();

        void settotalComprss(T value);
    }

    [Guid("F0000000-F000-F000-F000-F00000000501")]
    public interface IPvtD : IPvt<double>
    {
    }

    [Guid("F0000000-F000-F000-F000-F00000000502")]
    public interface IPvtF : IPvt<float>
    {
    }

    [Guid("F0000000-F000-F000-F000-F00000000600")]
    public interface IProductionDataRecord<T> : IUnknown
    {
        T gettime();

        void settime(T value);

        T getqo();

        void setqo(T value);

        T getqw();

        void setqw(T value);

        T getqg();

        void setqg(T value);

        T getqg_BOE();

        void setqg_BOE(T value);

        T getqt();

        void setqt(T value);
    }

    [Guid("F0000000-F000-F000-F000-F00000000601")]
    public interface IProductionDataRecordD : IProductionDataRecord<double>
    {
    }

    [Guid("F0000000-F000-F000-F000-F00000000602")]
    public interface IProductionDataRecordF : IProductionDataRecord<float>
    {
    }

    [Guid("F0000000-F000-F000-F000-F00000000700")]
    public interface IProductionData<T> : IUnknown
    {
        uint getRecordCount();

        void setRecordCount(uint value);

        IProductionDataRecord<T> getRecords();

        void setRecords(IProductionDataRecord<T> value);
    }

    [Guid("F0000000-F000-F000-F000-F00000000701")]
    public interface IProductionDataD : IProductionData<double>
    {
    }

    [Guid("F0000000-F000-F000-F000-F00000000702")]
    public interface IProductionDataF : IProductionData<float>
    {
    }

    [Guid("F0000000-F000-F000-F000-F00000000800")]
    public interface IMultiPorosityData<T> : IUnknown
    {
        IReservoirProperties<T> getReservoir();

        void setReservoir(IReservoirProperties<T> value);

        IWellProperties<T> getWell();

        void setWell(IWellProperties<T> value);

        IFractureProperties<T> getFracture();

        void setFracture(IFractureProperties<T> value);

        INaturalFractureProperties<T> getNaturalFracture();

        void setNaturalFracture(INaturalFractureProperties<T> value);

        IPvt<T> getPVT();

        void setPVT(IPvt<T> value);
    }

    [Guid("F0000000-F000-F000-F000-F00000000801")]
    public interface IMultiPorosityDataD : IMultiPorosityData<double>
    {
    }

    [Guid("F0000000-F000-F000-F000-F00000000802")]
    public interface IMultiPorosityDataF : IMultiPorosityData<float>
    {
    }

}