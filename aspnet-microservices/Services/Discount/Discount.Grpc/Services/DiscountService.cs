using AutoMapper;
using Discount.Grpc.Entities;
using Discount.Grpc.Protos;
using Discount.Grpc.Repositories;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Discount.Grpc.Services
{

    public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
    {
        #region ctor

        private readonly IDiscountRepository _discountRepository;
        private readonly ILogger<DiscountService> _logger;
        private readonly IMapper _mapper;
        public DiscountService(IDiscountRepository discountRepository, ILogger<DiscountService> logger,
            IMapper mapper)
        {
            _discountRepository = discountRepository;
            _logger = logger;
            _mapper = mapper;
        }

        #endregion

        #region methods

        #region get discount

        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await _discountRepository.GetDiscount(request.ProductName);
            if (coupon == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Discount with ProductName={request.ProductName} is not found."));
            }
            _logger.LogInformation("Discount is retrieved for product name");
            return _mapper.Map<CouponModel>(coupon);
        }


        #endregion

        #region create discount

        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var coupon = _mapper.Map<Coupon>(request.Coupon);

            await _discountRepository.CreateDiscount(coupon);

            _logger.LogInformation($"Discount is successfully created for product : {coupon.ProductName}");

            var couponModel = _mapper.Map<CouponModel>(coupon);
            return couponModel;
        }

        #endregion

        #region update discount

        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var coupon = _mapper.Map<Coupon>(request.Coupon);
            await _discountRepository.UpdateDiscount(coupon);

            _logger.LogInformation($"Discount is successfully updated. ProductName : {coupon.ProductName}");

            return _mapper.Map<CouponModel>(coupon);
        }

        #endregion

        #region delete discount

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var deleted = await _discountRepository.DeleteDiscount(request.ProductName);

            var response = new DeleteDiscountResponse
            {
                Success = deleted
            };

            return response;
        }

        #endregion

        #endregion
    }
}
