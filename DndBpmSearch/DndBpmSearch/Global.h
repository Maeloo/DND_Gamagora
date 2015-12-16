#pragma once
#include <cmath>
namespace Global
{
	inline float randf(float low, float high)
	{
		return low + (float)rand() / (float)(RAND_MAX / (high - low));
	}
}