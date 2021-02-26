using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiPorosity.Services.Optimization
{
    public sealed class LM_Config
    {
        public double covtol; /* Range tolerance for covariance calculation */

        public int douserscale; /* Scale variables by user values?
		                         1 = yes, user scale values in diag;
		                         0 = no, variables scaled internally */

        public double epsfcn; /* Finite derivative step size */

        public double ftol; /* Relative chi-square convergence criterium */

        public double gtol; /* Orthogonality convergence criterium */

        public int maxfev; /* Maximum number of function evaluations */

        public int maxiter; /* Maximum number of iterations.  If maxiter == 0,
                             then basic error checking is done, and parameter
                             errors/covariances are estimated based on input
                             parameter values, but no fitting iterations are done. */

        public int nofinitecheck; /* Disable check for infinite quantities from user?
			        0 = do not perform check
			        1 = perform check 
		             */

        public int nprint;

        public double stepfactor; /* Initial step bound */

        public double xtol; /* Relative parameter convergence criterium */

        //mp_iterproc iterproc; /* Placeholder pointer - must set to 0 */
    }
}