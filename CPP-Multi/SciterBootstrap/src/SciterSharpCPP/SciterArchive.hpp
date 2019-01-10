//
//  SciterArchive.hpp
//  SciterBootstrap
//
//  Created by Ramon Mendes on 09/04/17.
//  Copyright © 2017 MI Software. All rights reserved.
//

#include "sciter-x.h"
#include "aux-slice.h"


class SciterArchive
{
private:
	HSARCHIVE _har;
	
public:
	SciterArchive() : _har(0) {}
	
	void Open(aux::bytes res_array)
	{
		_har = SAPI()->SciterOpenArchive(res_array.start, res_array.length);
		assert(_har);
	}
	
	void Close()
	{
		assert(_har);
		SAPI()->SciterCloseArchive(_har);
	}
	
	aux::bytes Get(LPCWSTR path)
	{
		LPCBYTE pb = 0; UINT blen = 0;
		BOOL res = SAPI()->SciterGetArchiveItem(_har, path, &pb, &blen);
		assert(res);
		return aux::bytes(pb, blen);
	}
};
