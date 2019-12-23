using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagerApi.Helper
{
    public class JsonPatchManager
    {
        private readonly IMapper _mapper;

        public JsonPatchManager(IMapper mapper)
        {
            _mapper = mapper;
        }

        public InputModel Convert<InputModel, OutputModel>(
            JsonPatchDocument<InputModel> inputModel, OutputModel entityModel)
            where InputModel : class
            where OutputModel : class
        {
            var modelToPatch = _mapper.Map<InputModel>(entityModel);
            inputModel.ApplyTo(modelToPatch);

            return modelToPatch;
        }
    }
}
