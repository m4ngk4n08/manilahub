using System;
using System.Collections.Generic;
using System.Text;

namespace manilahub.core.Services.IServices
{
    public interface ICryptographyService
    {
        string SHA512(string value);
    }
}
