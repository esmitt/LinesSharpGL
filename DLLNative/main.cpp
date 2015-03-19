#include <iostream>
#include <vector>

typedef struct vec3
{
	float x, y, z;
	//vec3(){ x = y = z = 0; }
	//vec3(float x, float y, float z){ this->x = x; this->y = y; this->z = z; }
	//vec3(const vec3 &v){ x = v.x; y = v.y; z = v.z; }
	//vec3 operator = (const vec3 & v){ this->x = v.x; this->y = v.y; this->z = v.z; return *this; }
};

typedef struct line
{
	vec3 p1, p2;
	//line(){  }
	//line(const line& l){ this->p1 = l.p1; this->p2 = l.p2; }
};

typedef struct{
	float r, g, b, a;
}color4;

//extern "C" __declspec(dllexport)void getLineData(color4* pColor, line* pLines, int & nLines);
extern "C" __declspec(dllexport)void getLineData(float* pColor, float* pLines, int & nLines);
extern "C" __declspec(dllexport)int getData();

int getData()
{
	return 9;
}


void getLineData(float* pColor, float* pLines, int & nLines)
//void getLineData(color4* pColor, line* pLines, int & nLines)
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
		}

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