#include <iostream>
#include <limits>
#include <vector>

typedef struct 
{
	float x, y, z;
}vec3;

typedef struct 
{
	vec3 p1, p2;
}line;

typedef struct{
	float r, g, b, a;
}color4;

extern "C" __declspec(dllexport)void getLineData(float* pColor, float* pLines, int & nLines, float* fCenter, float & fScale);

void getLineData(float* pColor, float* pLines, int & nLines, float* fCenter, float & fScale)
{
	//source of lines
	std::vector<line> vLines;
	vLines.push_back(line{ { { -0.5f }, { -0.5f }, { -0.5f } }, { { 0.5f }, { -0.5f }, { -0.5f } } });
	vLines.push_back(line{ { { 0.5f }, { -0.5f }, { -0.5f } }, { { 0.5f }, { 0.5f }, { -0.5f } } });
	vLines.push_back(line{ { { 0.5f }, { 0.5f }, { -0.5f } }, { { -0.5f }, { 0.5f }, { -0.5f } } });
	vLines.push_back(line{ { { -0.5f }, { 0.5f }, { -0.5f } }, { { -0.5f }, { -0.5f }, { -0.5f } } });

	vLines.push_back(line{ { { -0.5f }, { -0.5f }, { 0.0f } }, { { 0.5f }, { -0.5f }, { 0.0f } } });
	vLines.push_back(line{ { { 0.5f }, { -0.5f }, { 0.0f } }, { { 0.5f }, { 0.5f }, { 0.0f } } });
	vLines.push_back(line{ { { 0.5f }, { 0.5f }, { 0.0f } }, { { -0.5f }, { 0.5f }, { 0.0f } } });
	vLines.push_back(line{ { { -0.5f }, { 0.5f }, { 0.0f } }, { { -0.5f }, { -0.5f }, { 0.0f } } });

	vLines.push_back(line{ { { 0.5f }, { -0.5f }, { 0.0f } }, { { 0.5f }, { -0.5f }, { -0.5f } } });
	vLines.push_back(line{ { { 0.5f }, { 0.5f }, { 0.0f } }, { { 0.5f }, { 0.5f }, { -0.5f } } });
	vLines.push_back(line{ { { -0.5f }, { -0.5f }, { 0.0f } }, { { -0.5f }, { -0.5f }, { -0.5f } } });
	vLines.push_back(line{ { { -0.5f }, { 0.5f }, { 0.0f } }, { { -0.5f }, { 0.5f }, { -0.5f } } });

	//now the color
	std::vector<color4> vColor;
	vColor.push_back(color4{ 1.0f, 0.0f, 0.0f, 1.0f });
	vColor.push_back(color4{ 0.0f, 1.0f, 0.0f, 1.0f });
	vColor.push_back(color4{ 0.0f, 0.0f, 1.0f, 1.0f });
	vColor.push_back(color4{ 1.0f, 1.0f, 0.0f, 1.0f });

	vColor.push_back(color4{ 0.0f, 1.0f, 1.0f, 1.0f });
	vColor.push_back(color4{ 1.0f, 0.0f, 1.0f, 1.0f });
	vColor.push_back(color4{ 1.0f, 1.0f, 1.0f, 1.0f });
	vColor.push_back(color4{ 0.5f, 0.5f, 0.5f, 1.0f });

	vColor.push_back(color4{ 1.0f, 0.0f, 0.0f, 1.0f });
	vColor.push_back(color4{ 0.0f, 1.0f, 0.0f, 1.0f });
	vColor.push_back(color4{ 0.0f, 0.0f, 1.0f, 1.0f });
	vColor.push_back(color4{ 1.0f, 1.0f, 0.0f, 1.0f });

	vColor.push_back(color4{ 0.0f, 1.0f, 1.0f, 1.0f });
	vColor.push_back(color4{ 1.0f, 0.0f, 1.0f, 1.0f });
	vColor.push_back(color4{ 1.0f, 1.0f, 1.0f, 1.0f });
	vColor.push_back(color4{ 0.5f, 0.5f, 0.5f, 1.0f });

	vColor.push_back(color4{ 1.0f, 0.0f, 0.0f, 1.0f });
	vColor.push_back(color4{ 0.0f, 1.0f, 0.0f, 1.0f });
	vColor.push_back(color4{ 0.0f, 0.0f, 1.0f, 1.0f });
	vColor.push_back(color4{ 1.0f, 1.0f, 0.0f, 1.0f });

	vColor.push_back(color4{ 0.0f, 1.0f, 1.0f, 1.0f });
	vColor.push_back(color4{ 1.0f, 0.0f, 1.0f, 1.0f });
	vColor.push_back(color4{ 1.0f, 1.0f, 1.0f, 1.0f });
	vColor.push_back(color4{ 0.5f, 0.5f, 0.5f, 1.0f });

	if (pColor && pLines){
		//just copy the values stored in a vector of line/color to the array
		nLines = 0;
		int iIndex = 0;
		float xMin, xMax, yMin, yMax, zMin, zMax;
		xMin = yMin = zMin = std::numeric_limits<float>::max();
		xMax = yMax = zMax = std::numeric_limits<float>::min();
		for each (line l in vLines)
		{
			pLines[iIndex + 0] = l.p1.x;
			pLines[iIndex + 1] = l.p1.y;
			pLines[iIndex + 2] = l.p1.z;
			pLines[iIndex + 3] = l.p2.x;
			pLines[iIndex + 4] = l.p2.y;
			pLines[iIndex + 5] = l.p2.z;
			nLines++;
			iIndex += 6;
			//found the min/max
			if (l.p1.x < xMin)
				xMin = l.p1.x;
			if (l.p1.x > xMax)
				xMax = l.p1.x;
			if (l.p2.x < xMin)
				xMin = l.p2.x;
			if (l.p2.x > xMax)
				xMax = l.p2.x;

			if (l.p1.y < yMin)
				yMin = l.p1.y;
			if (l.p1.y > yMax)
				yMax = l.p1.y;
			if (l.p2.y < yMin)
				yMin = l.p2.y;
			if (l.p2.y > yMax)
				yMax = l.p2.y;

			if (l.p1.z < zMin)
				zMin = l.p1.z;
			if (l.p1.z > zMax)
				zMax = l.p1.z;
			if (l.p2.z < zMin)
				zMin = l.p2.z;
			if (l.p2.z > zMax)
				zMax = l.p2.z;
		}
		fCenter[0] = (xMin + xMax)*0.5f;
		fCenter[1] = (yMin + yMax)*0.5f;
		fCenter[2] = (zMin + zMax)*0.5f;
		fScale = std::fmaxf(std::fmaxf(xMax - xMin, yMax - yMin), zMax - zMin);
		fScale = 1.f / fScale;
		iIndex = 0;
		for each (color4 c in vColor)
		{
			pColor[iIndex + 0] = c.r;
			pColor[iIndex + 1] = c.g;
			pColor[iIndex + 2] = c.b;
			pColor[iIndex + 3] = c.a;
			iIndex+=4;
		}
	}
	else
	{
		nLines = vLines.size();
	}
}