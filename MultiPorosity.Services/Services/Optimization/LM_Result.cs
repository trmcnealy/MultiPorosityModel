using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiPorosity.Services.Optimization
{
    /* Definition of results structure, for when fit completes */

    public sealed class LM_Result
    {
        public double bestnorm; /* Final chi^2 */

        public double[] covar; /* Final parameter covariance matrix
                      npar x npar array, or 0 if not desired */

        public int nfev; /* Number of function evaluations */

        public int nfree; /* Number of free parameters */

        public int nfunc; /* Number of residuals (= num. of data points) */

        public int niter; /* Number of iterations */

        public int npar; /* Total number of parameters */

        public int npegged; /* Number of pegged parameters */

        public double orignorm; /* Starting value of chi^2 */

        public double[] resid; /* Final residuals
                      nfunc-vector, or 0 if not desired */

        public int status; /* Fitting status code */

        public string version; /* MPFIT version string */

        public double[] xerror; /* Final parameter uncertainties (1-sigma)
                      npar-vector, or 0 if not desired */

        public LM_Result(int numParameters)
        {
            xerror = new double[numParameters];
            covar  = new double[numParameters * numParameters];
        }
    }
}