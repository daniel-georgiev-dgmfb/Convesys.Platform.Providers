using System;
using System.Threading.Tasks;

namespace Convesys.Platform.Cryptography.Certificates.Backchannel.Validation
{
    public abstract class BackchannelValidationRule : IBackchannelCertificateValidationRule
    {
        public async Task Validate(BackchannelCertificateValidationContext context, Func<BackchannelCertificateValidationContext, Task> next)
        {
            var validationResult = this.ValidateInternal(context);
            if (!validationResult)
                return;

            await next(context);
        }

        protected abstract bool ValidateInternal(BackchannelCertificateValidationContext context);
    }
}