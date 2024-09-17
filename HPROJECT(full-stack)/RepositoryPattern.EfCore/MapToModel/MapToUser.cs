using RepositoryPattern.Core.DTOs;
using RepositoryPattern.Core.Models;
using RepositoryPatternWithUOW.Core.DTOs;
using Riok.Mapperly.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPattern.EfCore.MapToModel
{
    [Mapper(EnumMappingStrategy=EnumMappingStrategy.ByName,EnumMappingIgnoreCase =true)]
    public partial class MapToUser
    {
        [MapperIgnoreSource("ProfilePicture")]
        public partial Doctor MapToDoctor(MakeDoctorProfileDto makeDoctorProfileDto);
        public partial Patient MapToPatient(SignUpDto signUpDto);
        public partial Patient MapToPatient(MakePatientAccountDto makePatientAccountDto);
        
       


    }
}
